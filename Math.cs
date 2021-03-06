using System.Linq;
using System.Text.RegularExpressions;

class Math
{
    public static class Converter
    {
        public static class Temperature
        {
            public static class Reaumur
            {

                public static double ToFahrenheit(double reaumur)
                {
                    return ((reaumur / 0.8) * 1.8) + 32;
                }

                public static double ToCelsius(double reaumur)
                {
                    return reaumur / 0.8;
                }
                public static double ToKelvin(double reaumur)
                {
                    return (reaumur / 0.8) + 273.15;
                }

                public static double ToRankine(double reaumur)
                {
                    return reaumur * 2.2500 + 491.67;
                }
            }

            public static class Rankine
                {
                    public static double ToFahrenheit(double rankine)
                    {
                        return (rankine - 491.67) + 32.00;
                    }

                    public static double ToCelsius(double rankine)
                    {
                        return (rankine - 491.67) / 1.8;
                    }

                    public static double ToKelvin(double rankine)
                    {
                        return ((rankine - 491.67) /1.8) + 273.15;
                    }
                    public static double ToReaumur(double rankine)
                    {
                        return (rankine - 491.67) / 2.25;
                    }
                }

            public static class Kelvin
                {
                    public static double ToCelsius(double kelvin)
                    {
                        return kelvin - 273.15;
                    }

                    public static double ToFahrenheit(double kelvin)
                    {
                        return (kelvin - 273.15) * 1.8 + 32.00;
                    }

                    public static double ToRankine(double kelvin)
                    {
                        return kelvin * 1.8;
                    }
                    public static double ToReaumur(double kelvin)
                    {
                        return (kelvin - 273.15) * 0.8;
                    }
                }

            public static class Celsius
                {
                    public static double ToKelvin(double celsius)
                    {
                        return celsius + 273.15;
                    }

                    public static double ToFahrenheit(double celsius)
                    {
                        return (celsius * 1.8) + 32;
                    }

                    public static double ToRankine(double celsius)
                    {
                        return (celsius * 1.8) + 491.67;
                    }

                    public static double ToReaumur(double celsius)
                    {
                        return celsius * 0.8;
                    }
                }

            public static class Fahrenheit
                {
                    public static double ToKelvin(double fahrenheit)
                    {
                        double x = 5; double y = 9; double z = (fahrenheit - (double)32) * (x / y) + (double)273.15;
                       return z;
                    }

                    public static double ToCelsius(double fahrenheit)
                    {
                        return (fahrenheit - 32) / 1.8;
                    }

                    public static double ToRankine(double fahrenheit)
                    {
                        double z = fahrenheit + (double)459.67;
                        return z;
                    }

                    public static double ToReaumur(double fahrenheit)
                    {
                        return (fahrenheit - (double)32) / (double)1.8 * (double)0.8;
                    }
                }
        }

        public static class Mass
            {
                public static class Ton
                {
                    public static double ToKilogramm(double ton)
                    {
                        return ton * 1000;
                    }
                    public static double ToGramm(double ton)
                    {
                        return ton * 1000000;
                    }
                }

                public static class Kilogramm
                {
                    public static double ToTon(double kilogramm)
                    {
                        return kilogramm / 1000;
                    }

                    public static double ToGramm(double kilogramm)
                    {
                        return kilogramm * 1000;
                    }
                }

                public static class Gramm
                {
                    public static double ToTon(double gramm)
                    {
                        return gramm / 1000000;
                    }

                    public static double ToKilogramm(double gramm)
                    {
                        return gramm / 1000;
                    }
                }
            }
    }

    public static double CalculateString(string task)
        {
            if (!new Regex(@"([\*\/\+][\*\/\+\)])").IsMatch(task) && !task.Contains("()") && !new Regex(@"\d{1,},\d{1,},").IsMatch(task))
            {
                string _task = task;
                Regex regex = new Regex(@"(\([0-9,\+\-\*\/]{1,}\))(?!.*\1)");

                if (new Regex(@"([0-9]\()").IsMatch(_task))
                {
                    string s = new Regex(@"([0-9]\()").Match(_task).ToString();
                    _task = _task.Replace(s, s.Substring(0, s.Length - 1) + "*)");
                }

                while (regex.IsMatch(_task))
                {
                    string intermediateTask = regex.Match(_task).ToString();
                    _task = _task.Replace(intermediateTask, CalculateString(intermediateTask.Substring(1, intermediateTask.Length - 2)).ToString());
                }

                regex = new Regex(@"(([^0-9]\-(\d{1,},\d{1,}|\d{1,})|(\d{1,},\d{1,}|\d{1,}))[\/\*](\-(\d{1,},\d{1,}|\d{1,})|(\d{1,},\d{1,}|\d{1,})))");
                string intermediateTask2 = regex.Match(_task).ToString();

                while (_task.Contains('*') || _task.Contains('/'))
                {
                    if (intermediateTask2.Contains("/"))
                    {
                        double Number1 = 0;
                        double Number2 = 0;

                        string[] splitted = intermediateTask2.Split('/');

                        if (splitted[0].Substring(0, 1) == "(" || splitted[0].Substring(0, 1) == "+" || (splitted[0].Length >= 2 && splitted[0].Substring(0, 2) == "--")) splitted[0] = splitted[0].Substring(1);
                        if (intermediateTask2.Substring(0, 1) == "(" || intermediateTask2.Substring(0, 1) == "+" || intermediateTask2.Substring(0, 2) == "--") intermediateTask2 = intermediateTask2.Substring(1);

                        Number1 = double.Parse(splitted[0]);
                        Number2 = double.Parse(splitted[1]);

                        if (Number2 != 0)
                            _task = _task.Replace(intermediateTask2, (Number1 / Number2).ToString());
                        else
                            return double.NaN;

                        intermediateTask2 = regex.Match(_task).ToString();
                    }
                    else if (intermediateTask2.Contains("*"))
                    {
                        double Number1 = 0;
                        double Number2 = 0;

                        string[] splitted = intermediateTask2.Split('*');

                        if (splitted[0].Substring(0, 1) == "(" || splitted[0].Substring(0, 1) == "+" || (splitted[0].Length >= 2 && splitted[0].Substring(0, 2) == "--")) splitted[0] = splitted[0].Substring(1);


                        Number1 = double.Parse(splitted[0]);
                        Number2 = double.Parse(splitted[1]);

                        _task = _task.Replace(intermediateTask2, (Number1 * Number2).ToString());

                        intermediateTask2 = regex.Match(_task).ToString();
                    }
                }

                regex = new Regex(@"((\-(\d{1,},\d{1,}|\d{1,})|(\d{1,},\d{1,}|\d{1,}))[\+\-](\-(\d{1,},\d{1,}|\d{1,})|(\d{1,},\d{1,}|\d{1,})))");
                intermediateTask2 = regex.Match(_task).ToString();

                if (!new Regex(@"^(\d+|\d+,\d+)$").IsMatch(_task.Substring(1)))
                {
                    while ((_task.Contains('-') || _task.Contains('+')) && !new Regex(@"^(\d+|\d+,\d+)$").IsMatch(_task.Substring(1)))
                    {
                        if (intermediateTask2.Contains("+"))
                        {
                            double Number1 = 0;
                            double Number2 = 0;

                            string[] splitted = intermediateTask2.Split('+');

                            if (splitted[0].Substring(0, 1) == "(") splitted[0] = splitted[0].Substring(1);

                            Number1 = double.Parse(splitted[0]);
                            Number2 = double.Parse(splitted[1]);

                            _task = _task.Replace(intermediateTask2, (Number1 + Number2).ToString());

                            intermediateTask2 = regex.Match(_task).ToString();
                        }
                        else if (intermediateTask2.Contains("-"))
                        {
                            double Number1 = 0;
                            double Number2 = 0;

                            string[] splitted = intermediateTask2.Split('-');

                            if (splitted[0] != "" && splitted[0].Substring(0, 1) == "(") splitted[0] = splitted[0].Substring(1);

                            if (splitted[0] == "") Number1 = -double.Parse(splitted[1]);
                            else Number1 = double.Parse(splitted[0]);
                            if (splitted.Count() == 4) Number2 = -double.Parse(splitted[3]);
                            else if (splitted[0] != "" && splitted.Count() == 3) Number2 = -double.Parse(splitted[2]);
                            else Number2 = double.Parse(splitted[1]);

                            _task = _task.Replace(intermediateTask2, (Number1 - Number2).ToString());

                            intermediateTask2 = regex.Match(_task).ToString();
                        }
                    }
                }
                return double.Parse(_task);
            }
            return double.NaN;
        }
}
