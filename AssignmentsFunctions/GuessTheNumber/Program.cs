using GuessTheNumber.Services;

var numberGenerator = new NumberGenerator();
var gameService = new GameService(numberGenerator);

gameService.Play();
