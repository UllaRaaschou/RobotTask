namespace RobotTask
{
    public record Board(int Columns, int Rows)
    {
        public static Board FromArray(string[] boardArray)
        {
            var columns = ArgumentParser.TryParseToPositive(boardArray[0], "ColumnCount");
            var rows = ArgumentParser.TryParseToPositive(boardArray[1], "RowCount");
            return new Board(columns, rows);
        }

        public void EnsurePositionIsWithinBoard(Position position)
        {
            int positionColumn = position.Column;
            int positionRow = position.Row;

            if (positionColumn > Columns || positionColumn < 1)
                throw new ArgumentException("PositionColumn is outside board");
            if (positionRow > Rows || positionRow < 1)
                throw new ArgumentException("PositionRow is outside board");
        }
    }

    public record Position(int Column, int Row, Direction Dir)
    {
        public static Position FromArray(string[] positionArray)
        {
            var x = ArgumentParser.TryParseToPositive(positionArray[0], "x-coordinate");
            var y = ArgumentParser.TryParseToPositive(positionArray[1], "y-coordinate");

            if (!Enum.TryParse<Direction>(positionArray[2], true, out var dir))
                throw new ArgumentException("The startDirection must be either 'N', 'E', 'S' or 'W' ");

            return new Position(x, y, dir);
        }
    }

    public record Command(Movement[] Movements)
    {
        public static Command FromArray(string[] commandArray)
        {
            var commands = string.Concat(commandArray).ToUpper();
            if (string.IsNullOrEmpty(commands))
                throw new ArgumentException("CommandInput must contain minimum 1 char");

            var movements = new List<Movement>();
            for (var i = 0; i < commands.Length; i++)
            {
                if (!Enum.TryParse<Movement>(commands[i].ToString(), true, out var command))
                    throw new ArgumentException("CommandInput must be either 'Forward', 'Backward', 'Right' or 'Left'");
                movements.Add(command);
            }
            return new Command(movements.ToArray());
        }
    }

    public enum Movement { F, B, R, L }

    public enum Direction { N, S, E, W }

    public static class ArgumentParser
    {
        public static int TryParseToPositive(string value, string fieldName)
        {
            if (!int.TryParse(value, out var result) || result < 1)
                throw new ArgumentException($"{fieldName} must be a positive integer");
            return result;
        }
    }


}

