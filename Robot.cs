namespace RobotTask
{
    public record RobotInput(Board Board, Position Position, Command Command)
    {
        public static RobotInput GetValidatedInput(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                throw new ArgumentException("Input cannot be null or empty");

            var parts = inputString.Split(',').Select(p => p.Trim()).ToArray();
            if (parts.Length >= 6)
            {
                var validBoard = Board.FromArray(parts[..2]);
                var validPosition = Position.FromArray(parts[2..5]);
                var validCommand = Command.FromArray(parts[5..]);

                validBoard.EnsurePositionIsWithinBoard(validPosition);

                return new RobotInput(validBoard, validPosition, validCommand);
            }
            throw new ArgumentException("Board values, start-coordinates incl. start-direction and actionsChars are all required values");
        }
    }

    public record RobotOutput(Position Position);

    public class Robot
    {
        public RobotOutput Move(RobotInput validInput)
        {
            Board board = validInput.Board;
            int column = validInput.Position.Column;
            int row = validInput.Position.Row;
            Direction direction = validInput.Position.Dir;
            Movement[] movements = validInput.Command.Movements;
            (int dx, int dy) = GetDelta(direction);

            foreach (var action in movements)
            {
                switch (action)
                {
                    case Movement.F:
                        column += dx;
                        row += dy;
                        board.EnsurePositionIsWithinBoard(new Position(column, row, direction));
                        break;
                    case Movement.B:
                        column -= dx;
                        row -= dy;
                        board.EnsurePositionIsWithinBoard(new Position(column, row, direction));
                        break;
                    case Movement.R:
                        direction = GetDirAfter_R_Rotation(direction);
                        (dx, dy) = GetDelta(direction);
                        break;
                    case Movement.L:
                        direction = GetDirAfter_L_Rotation(direction);
                        (dx, dy) = GetDelta(direction);
                        break;
                }
            }
            return new RobotOutput(new Position(column, row, direction));
        }

        private (int, int) GetDelta(Direction direction)
        {
            return direction switch
            {
                Direction.N => (0, -1),
                Direction.W => (-1, 0),
                Direction.S => (0, 1),
                Direction.E => (1, 0)
            };
        }

        private Direction GetDirAfter_R_Rotation(Direction direction)
        {
            return direction switch
            {
                Direction.N => Direction.E,
                Direction.E => Direction.S,
                Direction.S => Direction.W,
                Direction.W => Direction.N
            };
        }

        private Direction GetDirAfter_L_Rotation(Direction direction)
        {
            return direction switch
            {
                Direction.N => Direction.W,
                Direction.W => Direction.S,
                Direction.S => Direction.E,
                Direction.E => Direction.N
            };

        }
    }
}
