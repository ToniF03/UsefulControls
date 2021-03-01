using System.Linq;
using System.Text.RegularExpressions;

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
                    return (celsius * (9 / 5)) + 32;
                }
            }

            public static class Fahrenheit
            {
                public static double ToKelvin(double fahrenheit)
                {
                    return (fahrenheit - 32 / 1.8) - 273.15;
                }

                public static double ToCelsius(double fahrenheit)
                {
                    return fahrenheit - 32 / 1.8;
                }
            }
        }
    }

    public static double CalculateString(string task)
    {
        string _task = task;
        Regex regex = new Regex(@"(\([0-9,\+\-\*\/]{1,}\))(?!.*\1)");

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

                if (splitted[0].Substring(0, 1) == "(" || splitted[0].Substring(0, 1) == "+" || splitted[0].Substring(0, 2) == "--") splitted[0] = splitted[0].Substring(1);
                if (intermediateTask2.Substring(0, 1) == "(" || intermediateTask2.Substring(0, 1) == "+" || intermediateTask2.Substring(0, 2) == "--") intermediateTask2 = intermediateTask2.Substring(1);

                Number1 = double.Parse(splitted[0]);
                Number2 = double.Parse(splitted[1]);

                if (Number2 != 0)
                    _task = _task.Replace(intermediateTask2, (Number1 / Number2).ToString());
                else
                    _task = _task.Replace(intermediateTask2, (0).ToString());

                intermediateTask2 = regex.Match(_task).ToString();
            }
            else if (intermediateTask2.Contains("*"))
            {
                double Number1 = 0;
                double Number2 = 0;

                string[] splitted = intermediateTask2.Split('*');

                if (splitted[0].Substring(0, 1) == "(" || splitted[0].Substring(0, 1) == "+" || splitted[0].Substring(0, 2) == "--") splitted[0] = splitted[0].Substring(1);

                Number1 = double.Parse(splitted[0]);
                Number2 = double.Parse(splitted[1]);

                _task = _task.Replace(intermediateTask2, (Number1 * Number2).ToString());

                intermediateTask2 = regex.Match(_task).ToString();
            }
        }

        regex = new Regex(@"((\-(\d{1,},\d{1,}|\d{1,})|(\d{1,},\d{1,}|\d{1,}))[\+\-](\-(\d{1,},\d{1,}|\d{1,})|(\d{1,},\d{1,}|\d{1,})))");
        intermediateTask2 = regex.Match(_task).ToString();

        if (!new Regex(@"^\d+$").IsMatch(_task.Substring(1)))
        {
            while ((_task.Contains('-') || _task.Contains('+')) && !new Regex(@"^\d+$").IsMatch(_task.Substring(1)))
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
}
