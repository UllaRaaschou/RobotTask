namespace RobotTask.Test
{
    [TestClass]
    public class RobotTests
    {
        [TestMethod]
        // Rotation Right tests
        [DataRow("8,8,2,2,N,R", 2, 2, Direction.E)]     // Rotate right from North
        [DataRow("8,8,2,2,E,R", 2, 2, Direction.S)]     // Rotate right from East
        [DataRow("8,8,2,2,S,R", 2, 2, Direction.W)]     // Rotate right from South
        [DataRow("8,8,2,2,W,R", 2, 2, Direction.N)]     // Rotate right from West
        // Rotation Left tests
        [DataRow("8,8,2,2,N,L", 2, 2, Direction.W)]     // Rotate left from North
        [DataRow("8,8,2,2,W,L", 2, 2, Direction.S)]     // Rotate left from West
        [DataRow("8,8,2,2,S,L", 2, 2, Direction.E)]     // Rotate left from South
        [DataRow("8,8,2,2,E,L", 2, 2, Direction.N)]     // Rotate left from East
        // Forward from all directions
        [DataRow("8,8,4,4,N,F", 4, 3, Direction.N)]     // Move forward from North
        [DataRow("8,8,4,4,S,F", 4, 5, Direction.S)]     // Move forward from South
        [DataRow("8,8,4,4,W,F", 3, 4, Direction.W)]     // Move forward from West
        // Backward movement
        [DataRow("8,8,4,4,N,B", 4, 5, Direction.N)]     // Move backward once from North
        [DataRow("8,8,4,4,E,B", 3, 4, Direction.E)]     // Move backward once from East
        // Combination sequences - Basic
        [DataRow("8,8,2,3,N,FF", 2, 1, Direction.N)]       // Forward twice north
        [DataRow("8,8,2,2,E,FRF", 3, 3, Direction.S)]      // Forward-Right-Forward
        [DataRow("8,8,4,4,N,RFFLFB", 6, 4, Direction.N)]   // Right-Forward-Forward-Left-Forward-Backward
        [DataRow("8,8,4,4,N,RRRRR", 4, 4, Direction.E)]    // 5 right rotations
        [DataRow("8,8,2,2,E,FFFFFF", 8, 2, Direction.E)]   // 6 forward moves
        // Combination sequences - Advanced  
        [DataRow("8,8,4,4,W,LLFF", 6, 4, Direction.E)]     // 180° rotation then forward twice
        [DataRow("8,8,4,4,N,RRRR", 4, 4, Direction.N)]     // Full 360° rotation (4 rights)
        [DataRow("8,8,4,4,E,LLLL", 4, 4, Direction.E)]     // Full 360° rotation (4 lefts)
        [DataRow("8,8,2,2,N,FRFRFRFR", 2, 2, Direction.N)] // Square pattern returns to start
        [DataRow("8,8,5,5,N,FFRFF", 7, 3, Direction.E)]    // Forward-Forward-Right-Forward-Forward
        [DataRow("8,8,4,5,N,FFBBB", 4, 6, Direction.N)]    // Forward twice then backward three times
        [DataRow("8,8,3,3,E,RRFF", 1, 3, Direction.W)]         // Two rights then two forwards
        // Combination sequences - Very Complex (10+ command sequences, all carefully verified)
        [DataRow("8,8,4,4,N,RFFLFFRBBL", 4, 2, Direction.N)]   // 11 commands: complex navigation
        [DataRow("8,8,5,5,W,LLFFRRFF", 5, 5, Direction.W)]     // 8 commands: rotation dance returns to start
        [DataRow("8,8,2,2,E,FRFRFRFR", 2, 2, Direction.E)]     // 8 commands: square returns to start  
        [DataRow("8,8,3,4,W,LFRFBLRF", 2, 5, Direction.W)]     // 8 commands: zigzag pattern
        [DataRow("8,8,6,3,S,FRBLFRBLF", 8, 6, Direction.S)]    // 9 commands: complex spiral pattern
        public void TestMove_ReturnsOutputWithPosition(string inputString, int expEndXCoor, int expEndYCoor, Direction expEndDir)
        {
            // Arrange          
            RobotInput validInput = RobotInput.GetValidatedInput(inputString);
            Robot robot = new Robot();

            // Act
            var endPosition = robot.Move(validInput);

            // Assert
            Assert.AreEqual(endPosition.Position.Column, expEndXCoor);
            Assert.AreEqual(endPosition.Position.Row, expEndYCoor);
            Assert.AreEqual(endPosition.Position.Dir, expEndDir);
        }


        [TestMethod]
        [DataRow("1,1,1,1,E,R", 1, 1)]
        [DataRow("8,8,2,2,E,R", 8, 8)]
        [DataRow("24,36,2,2,E,R", 24, 36)]
        public void TestGetValidatedInput_SetsBoardColumnsAndRows(string inputStr, int expCol, int expRow)
        {
            // Act
            var validInput = RobotInput.GetValidatedInput(inputStr);

            // Assert;            
            Assert.AreEqual(expCol, validInput.Board.Columns);
            Assert.AreEqual(expRow, validInput.Board.Rows);
        }

        [TestMethod]
        [DataRow("", "Input cannot be null or empty")]
        [DataRow("-8,8,2,2,E,L", "ColumnCount must be a positive integer")]
        [DataRow("8,-8,2,2,E,L", "RowCount must be a positive integer")]
        [DataRow("E,8,2,2,E,L", "ColumnCount must be a positive integer")]
        [DataRow("8,E,2,2,E,L", "RowCount must be a positive integer")]
        [DataRow("8,2,2,E,L", "Board values, start-coordinates incl. start-direction and actionsChars are all required values")]
        public void TestGetValidatedInput_SetsBoardColumnsAndRows_ExceptionCases(string inputString, string errorMessage)
        {
            // ActAndAsset
            var ex = Assert.ThrowsExactly<ArgumentException>(() => RobotInput.GetValidatedInput(inputString));
            Assert.AreEqual(errorMessage, ex.Message);
        }


        [TestMethod]
        [DataRow("8,8,2,2,E,R", 2, 2, Direction.E)]
        [DataRow("8,8,2,2,e,R", 2, 2, Direction.E)]
        public void TestGetValidatedInput_SetsStartCoorAndStartDir(string inputStr, int expStartX, int expStartY, Direction expStartDir)
        {
            // Act
            var validInput = RobotInput.GetValidatedInput(inputStr);

            // Assert;          
            Assert.AreEqual(expStartX, validInput.Position.Column);
            Assert.AreEqual(expStartY, validInput.Position.Row);
            Assert.AreEqual(expStartDir, validInput.Position.Dir);
        }

        [TestMethod]
        [DataRow("8,8,-2,2,E,L", "x-coordinate must be a positive integer")]
        [DataRow("8,8,2,-2,E,L", "y-coordinate must be a positive integer")]
        [DataRow("8,8,2,2,J,L", "The startDirection must be either 'N', 'E', 'S' or 'W' ")]
        [DataRow("8,8,2,N,Left", "Board values, start-coordinates incl. start-direction and actionsChars are all required values")]
        public void TestGetValidatedInput_SetsStartCoorAndStartDir_ExceptionCases(string inputString, string errorMessage)
        {
            // ActAndAsset
            var ex = Assert.ThrowsExactly<ArgumentException>(() => RobotInput.GetValidatedInput(inputString));
            Assert.AreEqual(errorMessage, ex.Message);
        }


        [TestMethod]
        [DataRow("8,8,2,2,E,R", Movement.R)]
        [DataRow("8,8,2,2,E,r", Movement.R)]
        public void TestGetValidatedInput_SetsCommandMovementArray(string inputStr, Movement expAction)
        {
            // Act
            var validInput = RobotInput.GetValidatedInput(inputStr);

            // Assert;          
            Assert.AreEqual(expAction, validInput.Command.Movements[0]);
        }

        [TestMethod]
        [DataRow("8,8,2,2,E,P", "CommandInput must be either 'Forward', 'Backward', 'Right' or 'Left'")]
        [DataRow("8,8,2,2,E,", "CommandInput must contain minimum 1 char")]
        public void TestGetValidatedInput_SetsCommandMovementArray_ExceptionCases(string inputString, string errorMessage)
        {
            // ActAndAsset
            var ex = Assert.ThrowsExactly<ArgumentException>(() => RobotInput.GetValidatedInput(inputString));
            Assert.AreEqual(errorMessage, ex.Message);
        }


        [TestMethod]
        [DataRow("8,8,20,2,E,R", "PositionColumn is outside board")]      // Start position column > board width
        [DataRow("8,8,2,20,E,R", "PositionRow is outside board")]         // Start position row > board height
        [DataRow("8,8,-1,2,E,R", "x-coordinate must be a positive integer")]      // Start position column < 1 (negative)
        [DataRow("8,8,2,-1,E,R", "y-coordinate must be a positive integer")]         // Start position row < 1 (negative)
        public void TestEnsurePositionIsWithinBoard_ExceptionCases(string inputStr, string errorMessage)
        {
            // ArrangeAndAct
            var ex = Assert.ThrowsExactly<ArgumentException>(() => RobotInput.GetValidatedInput(inputStr));

            // Assert
            Assert.AreEqual(errorMessage, ex.Message);
        }

        [TestMethod]
        [DataRow("8,8,7,7,E,FF", "PositionColumn is outside board")]      // Move east beyond right edge
        [DataRow("8,8,1,1,W,F", "PositionColumn is outside board")]       // Move west beyond left edge
        [DataRow("8,8,4,1,N,F", "PositionRow is outside board")]          // Move north beyond top edge
        [DataRow("8,8,4,8,S,F", "PositionRow is outside board")]          // Move south beyond bottom edge
        [DataRow("8,8,1,8,E,B", "PositionColumn is outside board")]       // Move backward east (goes west) beyond left edge
        [DataRow("8,8,1,8,N,B", "PositionRow is outside board")]          // Move backward north (goes south) beyond bottom edge
        public void TestMove_ReturnsOutputWithPosition_ExceptionCases(string inputString, string errorMessage)
        {
            // Arrange                 
            RobotInput validInput = RobotInput.GetValidatedInput(inputString);
            Robot robot = new Robot();

            // Act
            var ex = Assert.ThrowsExactly<ArgumentException>(() => robot.Move(validInput));

            // Assert
            Assert.AreEqual(errorMessage, ex.Message);
        }
    }
}
