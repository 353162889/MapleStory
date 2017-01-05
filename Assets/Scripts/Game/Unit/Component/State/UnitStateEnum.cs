using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public enum UnitStateEnum : byte
    {
        Ground = 0,		//
        Float = 1,
        Climb = 2,
		Free = 3
    }

    public class UnitStateValue
    {
        private static int[] UnitStateValues = InitStateValues();

        private static int[] InitStateValues()
        {
            Array array = Enum.GetValues(typeof(UnitStateEnum));
            int[] result = new int[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (1 << (byte)array.GetValue(i));
            }
            return result;
        }

        public static void GetListState(int propState,ref List<UnitStateEnum> list)
        {
            list.Clear();
            int[] values = UnitStateValue.UnitStateValues;
            for (int i = 0; i < values.Length; i++)
            {
                if ((values[i] & propState) != 0)
                {
                    list.Add((UnitStateEnum)i);
                }
            }
        }

        public static bool HasState(int propState,UnitStateEnum state)
        {
            return (UnitStateValues[(byte)state] & propState) != 0;
        }
    }
}
