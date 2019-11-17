using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Equ
{
    class Equation
    {
        List<string> simpleLeft = new List<string>();
        List<string> simpleRight = new List<string>();
        string equLeft = "";
        string equRight = "";
        string finalEquation = "";
        double d, ValueForXcalculate = 0;
        string tempValue = "";

        public Equation(string[] args)
        {
            try
            {
                if (args[0] != "calc")
                {
                    Console.WriteLine("The Key word must be 'calc'! Example: Equ calc <Your Equation>");
                }
                else
                {

                    simpleLeftSide(args);
                    simpleRightSide(args);
                    //avoid to over flow the index of string "finalEquation"
                    finalEquation = equLeft + equRight + ' ';
                    if (!finalEquation.Contains('X'))
                    {
                        Console.WriteLine("Invalid Input!");

                    }
                    else
                    {
                        calculate(finalEquation);
                       // Console.WriteLine($"Final: {finalEquation}");
                    }

                }




            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input! Please check your input. It must be at least like the example 'ax^2 + bx + c = 0' or 'ax + b = 0'", e.Message);
            }
        }

        //The left side equation will be simplified as "ax^2 + bx (power) " or "ax2" or "ax" 
        //Because programme doesn't support characteric "^", all the "x^2" will be save into the string as "X2"
        private void simpleLeftSide(string[] @equation)
        {
            //int eLength = equation.Length;

            for (int i = 1; i < equation.Length; i++)
            {
                //deal with the first x and number
                if ((equation[i].Contains("X") || equation.Contains("X2")) && (i == 1) && (equation[i + 1] != "*" || equation[i + 1] != @"/"))
                {
                    simpleLeft.Add(equation[i]);
                    operatorControlLeft(equation[i + 1]);
                }
                else if (IsNumeric(equation[i]) && (i == 1) && (equation[i + 1] != "*" || equation[i + 1] != "/"))
                {
                    simpleLeft.Add(equation[i]);
                    operatorControlLeft(equation[i + 1]);
                }

                //deal with minus and plus
                if (equation[i] == "+" || equation[i] == "-")
                {
                    //deal with single x

                    if ((equation[i + 1].Contains("X") || equation[i + 1].Contains("X2") || IsNumeric(equation[i + 1])) && (equation[i + 1] != "*" || equation[i + 1] != "/"))
                    {
                        simpleLeft.Add(equation[i + 1]);
                        operatorControlLeft(equation[i + 2]);
                    }
                }
                // break point of left side
                if (equation[i] == "=")
                {

                    simpleLeft.Add(equation[i]);
                    break;
                }


                // Deal with time calculation
                if (equation[i] == "*")
                #region time for x
                {
                    //When element in the right side of "*" is x
                    if (equation[i + 1].Contains("X"))
                    {
                        //if the element in the left side of "*" is Number
                        if (IsNumeric(equation[i - 1]))
                        {
                            
                            if (equation[i + 1].Contains("X"))
                            {
                                ValueForXcalculate = Convert.ToDouble(equation[i - 1]) * GetNumberDouble(equation[i + 1]);
                                equation[i + 1] = Convert.ToString(ValueForXcalculate) + "X";

                            }
                            simpleLeft.Remove(equation[i]);
                            simpleLeft.Add(equation[i + 1]);
                            simpleLeft.Remove(equation[i - 1]);
                            operatorControlLeft(equation[i + 2]);

                            // Console.WriteLine("test!:equation[i+1] is {0}", equation[i + 1]);

                        }
                        

                    }
                    // when elements in both left and right side is number
                    else if (IsNumeric(equation[i - 1]) && IsNumeric(equation[i + 1]))
                    {
                        d = Convert.ToDouble(equation[i - 1]) * Convert.ToDouble(equation[i + 1]);
                        equation[i + 1] = Convert.ToString(d);
                        //Console.WriteLine($"the value of d is {d}");
                        tempValue = Convert.ToString(d);

                        if (d != 0)
                        {
                            simpleLeft.Add(Convert.ToString(d));

                        }
                        simpleLeft.Remove(equation[i]);
                        simpleLeft.Remove(equation[i - 1]);

                        operatorControlLeft(equation[i + 2]);

                    }
                    //when element in left side of "*" is x
                    else if (equation[i - 1].Contains("X"))
                    {

                        //if right side of "*" is number
                        if (IsNumeric(equation[i + 1]))
                        {
                            
                            if (equation[i - 1].Contains("X"))
                            {
                                ValueForXcalculate = Convert.ToDouble(equation[i + 1]) * GetNumberDouble(equation[i - 1]);
                                equation[i + 1] = Convert.ToString(ValueForXcalculate) + "X";

                            }
                            simpleLeft.Add(equation[i + 1]);
                            simpleLeft.Remove(equation[i - 1]);
                            simpleLeft.Remove(equation[i]);
                            operatorControlLeft(equation[i + 2]);

                        }
                    }

                }
                #endregion

                else if (equation[i] == "/")
                #region divide
                {
                    //When element in the right side of "/" is x
                    if (equation[i + 1].Contains("X"))
                    {
                        //if the element in the left side of "/" is Number
                        if (IsNumeric(equation[i - 1]))
                        {
                            
                            if (equation[i + 1].Contains("X"))
                            {
                                ValueForXcalculate = Convert.ToDouble(equation[i - 1]) / GetNumberDouble(equation[i + 1]);
                                equation[i + 1] = Convert.ToString(ValueForXcalculate) + "X";

                            }
                            simpleLeft.Remove(equation[i]);
                            simpleLeft.Add(equation[i + 1]);
                            simpleLeft.Remove(equation[i - 1]);
                            operatorControlLeft(equation[i + 2]);

                            //Console.WriteLine("test!:equation[i+1] is {0}", equation[i + 1]);

                        }
                    }
                    // When both left and right side of "/" are number
                    else if (IsNumeric(equation[i - 1]) && IsNumeric(equation[i + 1]))
                    {
                        if (Convert.ToDouble(equation[i + 1]) != 0)
                        {
                            d = Convert.ToDouble(equation[i - 1]) / Convert.ToDouble(equation[i + 1]);
                            equation[i + 1] = Convert.ToString(d);
                            //Console.WriteLine($"the value of d is {d}");
                            tempValue = Convert.ToString(d);
                        }
                        else
                        {
                            Console.WriteLine("Error! Zero can not be divided!");
                            break;
                        }


                        if (d != 0)
                        {
                            simpleLeft.Add(Convert.ToString(d));

                        }
                        simpleLeft.Remove(equation[i]);
                        simpleLeft.Remove(equation[i - 1]);

                        operatorControlLeft(equation[i + 2]);

                    }
                    //when element in left side of "/" is x
                    else if (equation[i - 1].Contains("X"))
                    {
                        //if right side of "/" is number
                        if (IsNumeric(equation[i + 1]))
                        {
                           
                            if (equation[i - 1].Contains("X"))
                            {
                                ValueForXcalculate = GetNumberDouble(equation[i - 1]) / Convert.ToDouble(equation[i + 1]);
                                equation[i + 1] = Convert.ToString(ValueForXcalculate) + "X";

                            }
                            simpleLeft.Add(equation[i + 1]);
                            simpleLeft.Remove(equation[i - 1]);
                            simpleLeft.Remove(equation[i]);
                            operatorControlLeft(equation[i + 2]);

                        }

                    }

                }
                #endregion
            }

            foreach (string element in simpleLeft)
            {

                equLeft += element;


            }
            //Console.WriteLine("left side is {0}", equLeft);
        }

        //The right side equation will be simplified as "ax + b "
       
        private void simpleRightSide(string[] @equation)
        {

            for (int i = equation.Length - 1; i > 0; i--)
            {
                //
                if (equation[i] == "=")
                {
                    break;
                }
                else
                //deal with the first x and number
                if ((equation[i].Contains("X")) && (i == (equation.Length - 1)) && (equation[i - 1] != "*" || equation[i - 1] != @"/"))
                {
                    simpleRight.Add(equation[i]);
                    operatorControlRight(equation[i - 1]);
                }
                else if (IsNumeric(equation[i]) && (i == (equation.Length - 1)) && (equation[i - 1] != "*" || equation[i - 1] != "/"))
                {
                    simpleRight.Add(equation[i]);
                    operatorControlRight(equation[i - 1]);
                }
                //deal with minus and plus
                if (equation[i] == "+" || equation[i] == "-")
                {
                    //deal with single x

                    if ((equation[i - 1].Contains("X")|| IsNumeric(equation[i - 1])) && (equation[i - 1] != "*" || equation[i - 1] != "/"))
                    {
                        simpleRight.Add(equation[i - 1]);
                        operatorControlRight(equation[i - 2]);
                    }
                }

                // Deal with time calculation
                if (equation[i] == "*")
                #region left side time 
                {
                    //When element in the right side of "*" is x
                    if (equation[i + 1].Contains("X"))
                    {
                        //if the element in the left side of "*" is Number
                        if (IsNumeric(equation[i - 1]))
                        {
                           
                            if (equation[i + 1].Contains("X"))
                            {
                                ValueForXcalculate = Convert.ToDouble(equation[i - 1]) * GetNumberDouble(equation[i + 1]);
                                equation[i - 1] = Convert.ToString(ValueForXcalculate) + "X";

                            }
                            simpleRight.Remove(equation[i]);
                            simpleRight.Add(equation[i - 1]);
                            simpleRight.Remove(equation[i + 1]);
                            operatorControlRight(equation[i - 2]);

                            // Console.WriteLine("test!:equation[i+1] is {0}", equation[i + 1]);

                        }
                        
                    }
                    // when elements in both left and right side is number
                    else if (IsNumeric(equation[i - 1]) && IsNumeric(equation[i + 1]))
                    {
                        // Console.WriteLine("TEST");
                        d = Convert.ToDouble(equation[i - 1]) * Convert.ToDouble(equation[i + 1]);
                        equation[i - 1] = Convert.ToString(d);
                        //Console.WriteLine($"the value of d is {d}");
                        tempValue = Convert.ToString(d);

                        if (d != 0)
                        {
                            simpleRight.Add(Convert.ToString(d));
                        }
                        simpleRight.Remove(equation[i]);
                        simpleRight.Remove(equation[i + 1]);
                        operatorControlRight(equation[i - 2]);

                    }
                    //when element in left side of "*" is x
                    else if (equation[i - 1].Contains("X"))
                    {
                        //if right side of "*" is number
                        if (IsNumeric(equation[i + 1]))
                        {
                            
                            if (equation[i - 1].Contains("X"))
                            {
                                ValueForXcalculate = Convert.ToDouble(equation[i + 1]) * GetNumberDouble(equation[i - 1]);
                                equation[i - 1] = Convert.ToString(ValueForXcalculate) + "X";

                            }
                            simpleRight.Add(equation[i - 1]);
                            simpleRight.Remove(equation[i + 1]);
                            simpleRight.Remove(equation[i]);
                            operatorControlRight(equation[i - 2]);

                        }
                    }
                }
                #endregion

                else if (equation[i] == "/")
                {
                    //When element in the right side of "/" is x
                    if (equation[i + 1].Contains("X"))
                    {
                        //if the element in the left side of "/" is Number
                        if (IsNumeric(equation[i - 1]))
                        {
                            
                            if (equation[i + 1].Contains("X"))
                            {
                                ValueForXcalculate = Convert.ToDouble(equation[i - 1]) / GetNumberDouble(equation[i + 1]);
                                equation[i - 1] = Convert.ToString(ValueForXcalculate) + "X";

                            }
                            simpleRight.Remove(equation[i]);
                            simpleRight.Add(equation[i - 1]);
                            simpleRight.Remove(equation[i + 1]);
                            operatorControlRight(equation[i - 2]);
                            //Console.WriteLine("test!:equation[i+1] is {0}", equation[i + 1]);
                        }
                        //if the element in the left side of "/" is x
                        else if (equation[i - 1].Contains("X"))
                        {
                            ValueForXcalculate = GetNumberDouble(equation[i - 1]) / GetNumberDouble(equation[i + 1]);
                            // if left of "/" is "X"
                            if (equation[i - 1].Contains("X"))
                            {
                                if (equation[i - 1].Contains("X"))
                                {
                                    equation[i - 1] = Convert.ToString(ValueForXcalculate);
                                }
                                
                            }
                            

                            simpleRight.Add(equation[i - 1]);
                            simpleRight.Remove(equation[i + 1]);
                            simpleRight.Remove(equation[i]);
                            operatorControlRight(equation[i - 2]);
                        }

                    }
                    // When both left and right side of "/" are number
                    else if (IsNumeric(equation[i - 1]) && IsNumeric(equation[i + 1]))
                    {
                        if (Convert.ToDouble(equation[i + 1]) != 0)
                        {
                            d = Convert.ToDouble(equation[i - 1]) / Convert.ToDouble(equation[i + 1]);
                            equation[i - 1] = Convert.ToString(d);
                            //Console.WriteLine($"the value of d is {d}");
                            tempValue = Convert.ToString(d);
                        }
                        else
                        {
                            Console.WriteLine("Error! Zero can not be divided!");
                            break;
                        }


                        if (d != 0)
                        {
                            simpleRight.Add(Convert.ToString(d));

                        }
                        simpleRight.Remove(equation[i]);
                        simpleRight.Remove(equation[i + 1]);

                        operatorControlRight(equation[i - 2]);

                    }

                    //when element in left side of "/" is x
                    else if (equation[i - 1].Contains("X"))
                    {
                        //if right side of "/" is number
                        if (IsNumeric(equation[i + 1]))
                        {
                            
                            if ( equation[i - 1].Contains("X"))
                            {
                                ValueForXcalculate = GetNumberDouble(equation[i - 1]) / Convert.ToDouble(equation[i + 1]);
                                equation[i - 1] = Convert.ToString(ValueForXcalculate) + "X";

                            }
                            simpleRight.Add(equation[i - 1]);
                            simpleRight.Remove(equation[i + 1]);
                            simpleRight.Remove(equation[i]);
                            operatorControlRight(equation[i - 2]);

                        }

                    }
                }

            }
            // reorder the sequence of List.simpleRight
            simpleRight.Reverse();
            foreach (string element in simpleRight)
            {
                equRight += element;
            }
            // Console.WriteLine($"Right equation {equRight}");

        }

        // This function will check the string is a number or not
        private bool IsNumeric(string str)
        {
            string temp = @"^[+-]?\d*(,\d{3})*(\.\d+)?$";
            Regex reg = new Regex(temp);
            if (reg.IsMatch(str))
            {
                return true;
            }
            else
            {
                // Console.WriteLine("Not a Number!");
                return false;
            }
        }

        // pick the numbers in front of x
        private double GetNumberDouble(string str)
        {

            string result = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != 'X')
                {
                    result += str[i];
                }
                else
                {
                    break;
                }
            }
            //if front number of x is empty, it will return 1.
            if (result.Length == 0)
            {
                return 1;
            }
            else
            {
                return double.Parse(result);
            }
        }

        // check the string is an operator or not
        private bool checkOperator(string str)
        {
            if (str == "+")
            {
                return true;
            }
            else if (str == "-")
            {
                return true;
            }
            else if (str == "*")
            {
                return true;
            }
            else if (str == "/")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //deal with the operator situation in list
        private void operatorControlLeft(string str)
        {
            if (checkOperator(str))
            {
                simpleLeft.Add(str);

            }
        }
        //deal with the operator situation in list
        private void operatorControlRight(string str)
        {
            if (checkOperator(str))
            {
                simpleRight.Add(str);

            }
        }

        //final calculation
        private void calculate(string equation)
        {
            //The equation will simplify as "ax+b=cx+d" 
            //variable "sign" will control the positive or negative
            // n is the length of the equation you give
            // j is a temporary position of the simplified equation
            int n = equation.Length, sign = 1, j = 0;
            double a = 0, b = 0, c = 0, x1, x2, delta;
            //Console.WriteLine($"Length of this equation is {n}");

            try
            {
                for (int i = 0; i < n; i++)
                {
                    if (equation[i] == '+' || equation[i] == '-')
                    {
                        if (i > j)
                        {
                            c += double.Parse(equation.Substring(j, i - j)) * sign;

                        }
                        j = i;
                    }
                    else if (equation[i] == 'X' && (i + 1 < n))
                    {
                        if (i == j || equation[i - 1] == '+')
                        {
                            b += sign;
                        }
                        else if (equation[i - 1] == '-')
                        {
                            b -= sign;
                        }
                        else
                        {
                            b += double.Parse(equation.Substring(j, i - j)) * sign;
                        }
                        j = i + 1;
                    }
                    else if (equation[i] == 'X' && (i + 1 < n))
                    {
                        if (i == j || equation[i - 1] == '+')
                        {
                            a += sign;
                        }
                        else if (equation[i - 1] == '-')
                        {
                            a -= sign;
                        }
                        else
                        {
                            a += double.Parse(equation.Substring(j, i - j)) * sign;
                        }
                        j = i + 2;
                    }
                    else if (equation[i] == '=')
                    {
                        //Console.WriteLine($"i={i}");
                        if (i > j) c += double.Parse(equation.Substring(j, i - j)) * sign;
                        //Console.WriteLine($"j={j}, capture={i - j},b={b}");
                        sign = -1;
                        j = i + 1;
                        //
                    }
                }
                if (j < n - 1)
                {
                    // Console.WriteLine("N is {0}",n);
                    c += double.Parse(equation.Substring(j)) * sign;

                }
                // if the equation is like "ax + b = 0"
                if (a == 0)
                {
                    if (b == 0 && b == c)
                    {
                        Console.WriteLine("Infinite Solution!");
                    }
                    else if (b == 0 && b != c)
                    {
                        Console.WriteLine("No solution!");
                    }
                    else
                    {
                        double result = (-c / b);
                        Console.WriteLine($"x = {result}");
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input!", e.Message);
            }

        }
    }
}
