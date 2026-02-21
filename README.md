# Robot Task - Requirements Overview

## Grid
- The grid consists of fields arranged in a grid pattern
- Origin point is in the top-left corner
- Both rows (downward) and columns (to the right) are numbered sequentially with integers
- The grid always starts at field (1,1)
- No fields on the grid can have negative coordinates
 Grid Size
- The grid has a user-specified input value, indicated as e.g. `(7,8)`
- **Validation:** If input values for the grid are specified as anything other than 2 positive integers, an `ArgumentException` must be thrown 

## Robot Starting Position:
- The robot's starting position is a user-specified input value
- It is specified with 2 coordinates and a direction enum: `(N, E, S, W)`, representing a starting direction.
- E.g. `(2, 2, N)`
- **Validation:** 
  - If the robot's starting coordinates are specified with anything other than 2 positive integers and an enum being either 'N', 'E', 'S', or 'W', an `ArgumentException` must be thrown
  - If the user-specified position is outside the grid, an `ArgumentException` must be thrown 
	
## Commands:
- The robot's command is a user-specified input value
- Specified as a string containing the enums: `Forward`, `Backward`, `Right`, `Left`
- Each enum represents its own movement
- **Validation:**
  - If the robot's actions contain values other than 'Forward', 'Backward', 'Right', or 'Left', an `ArgumentException` must be thrown
  - If the robot moves outside the grid during execution of the user's command, an `ArgumentException` must be thrown 
	
## Assumptions:
- The user must always specify values for:
  - Grid
  - Starting position
  - At least one action