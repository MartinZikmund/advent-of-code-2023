// x * (totalTime - x) > record
// x * totalTime - x^2 > record
// x^2 - x * totalTime + (record + 1) = 0

var input = File.ReadAllLines("input.txt");

var totalTime = long.Parse(input[0].Substring("Time:".Length).Replace(" ", string.Empty));
var record = long.Parse(input[1].Substring("Distance:".Length).Replace(" ", string.Empty));

var d = totalTime * totalTime - 4 * (record + 1);
var x1 = (totalTime + Math.Sqrt(d)) / 2;
var x2 = (totalTime - Math.Sqrt(d)) / 2;

var time = (long)Math.Ceiling(Math.Min(x1, x2));
var waysToWin = (totalTime - time) - time + 1;

Console.WriteLine(waysToWin);