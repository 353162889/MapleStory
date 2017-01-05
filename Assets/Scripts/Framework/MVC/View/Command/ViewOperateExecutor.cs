using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public class ViewOperateExecutor : CommandDynamicSequence
	{
		private static Dictionary<CommandViewType,Action<CommandBaseView>> _map = new Dictionary<CommandViewType, Action<CommandBaseView>>{
			{CommandViewType.Init,SaveInitCmd},
			{CommandViewType.Destroy,SaveDestroyCmd},
			{CommandViewType.Open,SaveOpenCmd},
			{CommandViewType.Close,SaveCloseCmd}
		};

		public static void SaveInitCmd(CommandBaseView cmdView)
		{
			ObjectPool<CommandInitView>.Instance.SaveObject ((CommandInitView)cmdView);
		}
		public static void SaveDestroyCmd(CommandBaseView cmdView)
		{
			ObjectPool<CommandDestroyView>.Instance.SaveObject ((CommandDestroyView)cmdView);
		}
		public static void SaveOpenCmd(CommandBaseView cmdView)
		{
			ObjectPool<CommandOpenView>.Instance.SaveObject ((CommandOpenView)cmdView);
		}
		public static void SaveCloseCmd(CommandBaseView cmdView)
		{
			ObjectPool<CommandCloseView>.Instance.SaveObject ((CommandCloseView)cmdView);
		}

		public static T CreateCommand<T>() where T : CommandBaseView
		{
			return ObjectPool<T>.Instance.GetObject ();
		}

		public ViewOperateExecutor(bool childFailStop = true, bool isAutoDestroy = false)
            :base(childFailStop,isAutoDestroy)
		{
			ObjectPool<CommandOpenView>.Instance.Init (5);
			ObjectPool<CommandCloseView>.Instance.Init (5);
			ObjectPool<CommandDestroyView>.Instance.Init (2);
			ObjectPool<CommandInitView>.Instance.Init (2);
		}

		protected override void OnChildDestroy (CommandBase command)
		{
			base.OnChildDestroy (command);
			CommandBaseView cmdView = (CommandBaseView)command;
			CLog.Log ("[ViewOperateExecutor]OnChildDestroy:"+cmdView.CmdType);
			SaveCmd (cmdView);
		}

		protected void SaveCmd(CommandBaseView cmdView)
		{
			Action<CommandBaseView> action;
			_map.TryGetValue (cmdView.CmdType, out action);
			if (action != null)
			{
				action.Invoke (cmdView);
			}
		}

		public override void AddSubCommand (CommandBase command)
		{
			if (command is CommandBaseView)
			{
				CommandBaseView cmd = (CommandBaseView)command;
				if (HandCommand (cmd))
				{
					CLog.Log ("[ViewOperateExecutor]Add:"+cmd.CmdType);
					base.AddSubCommand (cmd);
				}
			}
		}

        private bool HandCommand(CommandBaseView cmdView)
		{
            //当前只有一个正在执行的命令
            if (IsExecuting)
            {
                if (_children.Count == 0)
                {
                    CommandBaseView executeChildView = (CommandBaseView)_executeChild;
                    //如果命令相同，过滤掉当前命令
                    if (executeChildView.CmdType == cmdView.CmdType)
                    {
                        SaveCmd(cmdView);
                        return false;
                    }
                    //如果当前正在执行销毁命令(延时销毁中)，那么取消当前销毁命令
                    if (executeChildView.CmdType == CommandViewType.Destroy)
                    {
                        SkipExecuteChild();
                        return true;
                    }
                }
                else
                {
                    LinkedListNode<CommandBase> node = _children.First;
                    while (node != null)
                    {
                        CommandBaseView cmdChild = (CommandBaseView)node.Value;
                        //如果期间有销毁命令，将其移除
                        if (cmdChild.CmdType == CommandViewType.Destroy)
                        {
                            node = node.Next;
                            _children.Remove(cmdChild);
                            OnChildDestroy(cmdChild);
                            continue;
                        }
                        else
                        {
                            int cmdChildValue = (int)cmdChild.CmdType;
                            int cmdViewValue = (int)cmdView.CmdType;
                            //如果两个命令可以抵消，移除
                            if (cmdChildValue + cmdViewValue == 0)
                            {
                                _children.Remove(cmdChild);
                                OnChildDestroy(cmdChild);
                                SaveCmd(cmdView);
                                return false;
                            }
                            //如果最后一个命令与将要加入的命令相同，使用最新的命令
                            else if (node.Next == null && cmdChild.CmdType == cmdView.CmdType)
                            {
                                _children.Remove(cmdChild);
                                OnChildDestroy(cmdChild);
                                return true;
                            }
                        }
                        node = node.Next;
                    }
                    return true;
                }
            }
            return true;
		}

    }
}

