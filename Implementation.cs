using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    class Implementation
    {
        static public int calculate(Node left, string operation, Node right)
        {   
            throw new NotImplementedException();

            // TODO: implement me!

            int lValue = 0; // left.getValue();
            int rValue = 0; // right.getValue();
            switch (operation)
            {
                case "+":
                    return lValue + rValue;
                case "-":
                    return lValue - rValue;
                case "*":
                    return lValue * rValue;
                case "/":
                    return lValue / rValue;
                case "<":
                    return Convert.ToInt32(lValue < rValue);
                case "<=":
                    return Convert.ToInt32(lValue <= rValue);
                case ">":
                    return Convert.ToInt32(lValue > rValue);
                case ">=":
                    return Convert.ToInt32(lValue >= rValue);
                case "=":
                    return Convert.ToInt32(lValue == rValue);
                case "!=":
                    return Convert.ToInt32(lValue != rValue);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
