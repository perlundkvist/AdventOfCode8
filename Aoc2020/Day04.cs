namespace AdventOfCode8.Aoc2020
{
    class Day04 : DayBase
    {

        private List<string> EyeColors = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        public void Run()
        {
            var puzzleInput = GetInput("2020_04");
            var fields = new List<string>();
            var lines = new List<string>();
            var invalid = 0;
            var valid = 0; // 181 too low
            var passports = 0;
            var lineNo = 0;
            var lineValid = true;
            foreach (var line in puzzleInput)
            {
                lineNo++;
                lines.Add(line);
                if (string.IsNullOrEmpty(line.Trim()))
                {
                    if (lineValid && CheckFields(fields))
                        valid++;
                    else
                    {
                        invalid++;
                        Console.WriteLine($"Invalid passport {passports + 1}, line {lineNo}, fields {string.Join(',', fields)}");
                        lines.ForEach(l => Console.WriteLine(l));
                    }
                    lineValid = true;
                    fields.Clear();
                    lines.Clear();
                    passports++;
                    continue;
                }
                var split = line.Split(' ');
                foreach (var field in split)
                {
                    fields.Add(field.Split(':')[0]);
                    if (lineValid)
                    {
                        lineValid = CheckFieldValue(field);
                        if (!lineValid)
                            Console.WriteLine($"Failed on {field}");
                    }
                }
            }
            Console.WriteLine($"Valid {valid}, Invalid: {invalid}, passports {passports}");
         }

        private bool CheckFieldValue(string field)
        {
            var name = field.Split(':')[0];
            var value = field.Split(':')[1];

            switch (name)
            {
                case "byr":
                    return CheckIntValue(value, 1920, 2002, 4);
                case "iyr":
                    return CheckIntValue(value, 2010, 2020, 4);
                case "eyr":
                    return CheckIntValue(value, 2020, 2030, 4);
                case "hgt":
                    if (value.EndsWith("cm"))
                        return CheckIntValue(value.Substring(0, value.Length-2), 150, 193);
                    if (value.EndsWith("in"))
                        return CheckIntValue(value.Substring(0, value.Length - 2), 59, 76);
                    return false;
                case "hcl":
                    if (value.Length != 7 || !value.StartsWith('#'))
                        return false;
                    var result= int.TryParse(value.Substring(1), System.Globalization.NumberStyles.HexNumber, null, out var hcl);
                    return result;
                case "ecl":
                    return EyeColors.Contains(value);
                case "pid":
                    if (value.Length != 9)
                        return false;
                    return long.TryParse(value, out var pid);
            }
            return true;
        }

        private bool CheckIntValue(string value, int min, int max, int? lenght = null)
        {
            if (lenght.HasValue && value.Length != lenght.Value)
                return false;
            if (!int.TryParse(value, out var result))
                return false;
            return result >= min && result <= max;
        }

        private bool CheckFields(List<string> fields)
        {
            if (!CheckField("byr", fields)) 
                return false;
            if (!CheckField("iyr", fields))
                return false;
            if (!CheckField("eyr", fields))
                return false;
            if (!CheckField("hgt", fields))
                return false;
            if (!CheckField("hcl", fields))
                return false;
            if (!CheckField("ecl", fields))
                return false;
            if (!CheckField("pid", fields))
                return false;

            return true;
        }

        private bool CheckField(string field, List<string> fields)
        {
            if (fields.Contains(field))
                return true;
            Console.WriteLine($"Missing {field}");
            return false;
        }
    }
}
