using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagLib.Utils
{
    class BitManipulator
    {
        public static void ClearBit(ref object MyByte, object MyBit)
        {
            short num = Conversions.ToShort(Operators.ExponentObject((object)2, Operators.SubtractObject(MyBit, (object)1)));
            MyByte = Operators.AndObject(MyByte, (object)~num);
        }

        public static bool ExamineBit(object MyByte, object MyBit)
        {
            short num = Conversions.ToShort(Operators.ExponentObject((object)2, Operators.SubtractObject(MyBit, (object)1)));
            return Operators.ConditionalCompareObjectGreater(Operators.AndObject(MyByte, (object)num), (object)0, false);
        }

        public static void SetBit(ref object MyByte, object MyBit)
        {
            short num = Conversions.ToShort(Operators.ExponentObject((object)2, Operators.SubtractObject(MyBit, (object)1)));
            MyByte = Operators.OrObject(MyByte, (object)num);
        }

        public static void ToggleBit(ref object MyByte, object MyBit)
        {
            short num = Conversions.ToShort(Operators.ExponentObject((object)2, Operators.SubtractObject(MyBit, (object)1)));
            MyByte = Operators.XorObject(MyByte, (object)num);
        }
    }
}
