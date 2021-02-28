using System;

class Math
    {
        public static class Converter
        {
            public static class Temperature
            {
                public static class Kelvin
                {
                    public static double ToCelsius(double kelvin)
                    {
                        return kelvin + 273.15;
                    }

                    public static double ToFahrenheit(double kelvin)
                    {
                        return 0;
                    }
                }

                public static class Celsius
                {
                    public static double ToKelvin(double celsius)
                    {
                        return celsius - 273.15;
                    }

                    public static double ToFahrenheit(double celsius)
                    {
                        return (celsius * (9/5)) + 32;
                    }
                }
            }
        }

        public static double CalculateString(string task)
        {
            double _result = 0;
            string _task = task;

            while (_task.Contains("(") && _task.Contains(")"))
            {
                for (int Index = _task.Length - 1; Index >= 0; Index--)
                {
                    if (_task.Substring(Index, 1) == "(")
                    {
                        for (int offset = 1; _task.Substring(Index + offset - 1, 1) != ")"; offset++)
                        {
                            if (_task.Substring(Index + offset, 1) == ")")
                            {
                                Console.WriteLine("Task in brackets: " + _task.Substring(Index + 1, offset - 1));
                                //Console.WriteLine("Result of task in brackets: " + CalculateString(_task.Substring(Index + 1, offset - 1)).ToString());

                                string taskPart1 = _task.Substring(0, Index);
                                string taskPart2 = CalculateString(_task.Substring(Index + 1, offset - 1)).ToString();
                                string taskPart3 = _task.Substring(Index + offset, _task.Length - (Index + offset + 1));

                                _task = taskPart1 + taskPart2 + taskPart3;
                                break;
                                //Console.WriteLine(taskPart1 + taskPart2 + taskPart3);
                            }
                        }
                        break;
                    }
                }
            }

            while (_task.Contains("*") || _task.Contains("/"))
            {
                for (int Index = 0; Index < _task.Length; Index++)
                {
                    #region "Multiply"
                    if (_task.Substring(Index, 1) == "*")
                    {
                        int offsetRight = 0;
                        int offsetLeft = 0;

                        double interimResult = 0;

                        double Number1 = 0;
                        double Number2 = 0;

                        for (int offset = 1; offset < _task.Length; offset++)
                        {
                            if (Index + offset + 1 != _task.Length)
                            {
                                string pieceOfTask = _task.Substring(Index + offset + 1, 1);
                                if (pieceOfTask == "*" || pieceOfTask == "/" || pieceOfTask == "+" || pieceOfTask == "-" || offset + 1 == _task.Length - Index)
                                {
                                    offsetRight = offset;
                                    break;
                                }
                            }
                            else
                            {
                                offsetRight = offset;
                                break;
                            }
                        }

                        for (int offset = 1; Index - offset >= 0; offset++)
                        {
                            if (Index - offset - 1 != -1)
                            {
                                string pieceOfTask = _task.Substring(Index - offset - 1, 1);
                                if (pieceOfTask == "*" || pieceOfTask == "/" || pieceOfTask == "+" || pieceOfTask == "-" || Index - offset == 0)
                                {
                                    offsetLeft = offset;
                                    break;
                                }
                            }
                            else
                            {
                                offsetLeft = offset;
                                break;
                            }
                        }

                        string n1 = _task.Substring(Index - offsetLeft, offsetLeft);
                        string n2 = _task.Substring(Index + 1, offsetRight);

                        if (n2.EndsWith(")")) n2 = n2.Substring(0, n2.Length - 1);

                        Number1 = double.Parse(n1);
                        Number2 = double.Parse(n2);

                        interimResult = Number1 * Number2;

                        string taskPart1 = _task.Substring(0, Index - offsetLeft);
                        string taskPart2 = _task.Substring(Index + offsetRight + 1, (_task.Length - 1) - (Index + offsetRight));

                        _task = taskPart1 + interimResult.ToString() + taskPart2;
                        break;
                    }
                    #endregion
                    #region "Divide"
                    else if (_task.Substring(Index, 1) == "/")
                    {
                        int offsetRight = 0;
                        int offsetLeft = 0;

                        double interimResult = 0;

                        double Number1 = 0;
                        double Number2 = 0;

                        for (int offset = 1; offset < _task.Length; offset++)
                        {
                            if (Index + offset + 1 != _task.Length)
                            {
                                string pieceOfTask = _task.Substring(Index + offset + 1, 1);
                                if (pieceOfTask == "*" || pieceOfTask == "/" || pieceOfTask == "+" || pieceOfTask == "-" || offset + 1 == _task.Length - Index)
                                {
                                    offsetRight = offset;
                                    break;
                                }
                            }
                            else
                            {
                                offsetRight = offset;
                                break;
                            }
                        }

                        for (int offset = 1; Index - offset >= 0; offset++)
                        {
                            if (Index - offset - 1 != -1)
                            {
                                string pieceOfTask = _task.Substring(Index - offset - 1, 1);
                                if (pieceOfTask == "*" || pieceOfTask == "/" || pieceOfTask == "+" || pieceOfTask == "-" || Index - offset == 0)
                                {
                                    offsetLeft = offset;
                                    break;
                                }
                            }
                            else
                            {
                                offsetLeft = offset;
                                break;
                            }
                        }

                        string n1 = _task.Substring(Index - offsetLeft, offsetLeft);
                        string n2 = _task.Substring(Index + 1, offsetRight);

                        Number1 = double.Parse(n1);
                        Number2 = double.Parse(n2);

                        if (Number2 != 0)
                        {
                            interimResult = Number1 / Number2;

                            string taskPart1 = _task.Substring(0, Index - offsetLeft);
                            string taskPart2 = _task.Substring(Index + offsetRight + 1, (_task.Length - 1) - (Index + offsetRight));

                            _task = taskPart1 + interimResult.ToString() + taskPart2;
                        }
                        else
                            _task = "0";
                        break;
                    }
                    #endregion
                }
            }

            while (_task.Contains("+") || _task.Contains("-"))
            {
                for (int Index = 0; Index < _task.Length; Index++)
                {
                    #region "Add"
                    if (_task.Substring(Index, 1) == "+")
                    {
                        int offsetRight = 0;
                        int offsetLeft = 0;

                        double interimResult = 0;

                        double Number1 = 0;
                        double Number2 = 0;

                        for (int offset = 1; offset < _task.Length; offset++)
                        {
                            if (Index + offset + 1 != _task.Length)
                            {
                                string pieceOfTask = _task.Substring(Index + offset + 1, 1);
                                if (pieceOfTask == "*" || pieceOfTask == "/" || pieceOfTask == "+" || pieceOfTask == "-" || offset + 1 == _task.Length - Index)
                                {
                                    offsetRight = offset;
                                    break;
                                }
                            }
                            else
                            {
                                offsetRight = offset;
                                break;
                            }
                        }

                        for (int offset = 1; Index - offset >= 0; offset++)
                        {
                            if (Index - offset - 1 != -1)
                            {
                                string pieceOfTask = _task.Substring(Index - offset - 1, 1);
                                if (pieceOfTask == "*" || pieceOfTask == "/" || pieceOfTask == "+" || pieceOfTask == "-" || Index - offset == 0)
                                {
                                    offsetLeft = offset;
                                    break;
                                }
                            }
                            else
                            {
                                offsetLeft = offset;
                                break;
                            }
                        }

                        string n1 = _task.Substring(Index - offsetLeft, offsetLeft);
                        string n2 = _task.Substring(Index + 1, offsetRight);

                        Number1 = double.Parse(n1);
                        Number2 = double.Parse(n2);

                        interimResult = Number1 + Number2;

                        string taskPart1 = _task.Substring(0, Index - offsetLeft);
                        string taskPart2 = _task.Substring(Index + offsetRight + 1, (_task.Length - 1) - (Index + offsetRight));

                        _task = taskPart1 + interimResult.ToString() + taskPart2;
                        break;
                    }
                    #endregion
                    #region "Subtract"
                    else if (_task.Substring(Index, 1) == "-")
                    {
                        int offsetRight = 0;
                        int offsetLeft = 0;

                        double interimResult = 0;

                        double Number1 = 0;
                        double Number2 = 0;

                        for (int offset = 1; offset < _task.Length; offset++)
                        {
                            if (Index + offset + 1 != _task.Length)
                            {
                                string pieceOfTask = _task.Substring(Index + offset + 1, 1);
                                if (pieceOfTask == "*" || pieceOfTask == "/" || pieceOfTask == "+" || pieceOfTask == "-" || offset + 1 == _task.Length - Index)
                                {
                                    offsetRight = offset;
                                    break;
                                }
                            }
                            else
                            {
                                offsetRight = offset;
                                break;
                            }
                        }

                        for (int offset = 1; Index - offset >= 0; offset++)
                        {
                            if (Index - offset - 1 != -1)
                            {
                                string pieceOfTask = _task.Substring(Index - offset - 1, 1);
                                if (pieceOfTask == "*" || pieceOfTask == "/" || pieceOfTask == "+" || pieceOfTask == "-" || Index - offset == 0)
                                {
                                    offsetLeft = offset;
                                    break;
                                }
                            }
                            else
                            {
                                offsetLeft = offset;
                                break;
                            }
                        }

                        string n1 = _task.Substring(Index - offsetLeft, offsetLeft);
                        string n2 = _task.Substring(Index + 1, offsetRight);

                        Number1 = double.Parse(n1);
                        Number2 = double.Parse(n2);

                        interimResult = Number1 - Number2;

                        string taskPart1 = _task.Substring(0, Index - offsetLeft);
                        string taskPart2 = _task.Substring(Index + offsetRight + 1, (_task.Length - 1) - (Index + offsetRight));

                        _task = taskPart1 + interimResult.ToString() + taskPart2;
                        break;
                    }
                    #endregion
                }
            }

            if (_task == "") _task = "0";
            Console.WriteLine("Result of the task:  " + _task);
            _result = double.Parse(_task);
            return _result;
        }
    }
