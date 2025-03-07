// See https://aka.ms/new-console-template for more information

string[] sounds = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "B", "H"];
int[] majorShifts = [2, 2, 1, 2, 2, 2, 1];

var root = Console.ReadLine()!;
var index = sounds.ToList().IndexOf(root);

var clshift = index;
Console.Write(root);
foreach (var shift in majorShifts)
{
    clshift += shift;
    Console.Write(" " + sounds[clshift%12]);
}

