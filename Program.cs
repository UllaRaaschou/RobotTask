using RobotTask;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();


app.MapPost("/api/robot/move", (RobotInputRequest req) =>
{
    try
    {
        var validInput = RobotInput.GetValidatedInput(req.Raw);
        var robot = new Robot();
        var result = robot.Move(validInput);
        var dto = new RobotMoveResultDto(
            new PositionDto(
                result.Position.Column,
                result.Position.Row,
                result.Position.Dir.ToString()
            )
        );
        return Results.Ok(dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new
        {
            error = ex.Message,
            message = "Invalid input. Expected: 'cols,rows,x,y,dir,actions' f.eks. '8,8,2,2,E,R'."
        });
    }
});

app.Run();


public record RobotInputRequest(string Raw);

public record PositionDto(int Column, int Row, string Direction);

public record RobotMoveResultDto(PositionDto Position);


