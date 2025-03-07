using System.Diagnostics;

var filename = args[0];

var sr = new StreamReader($"../../../{filename}");
float max = 0;
var line = -1;
var cline = 0;
while (!sr.EndOfStream && line == -1) {
    var pnum = sr.ReadLine()!;
    try {
        var num = float.Parse(pnum);
        line = cline;
        max = num;
    }
    catch (Exception e)
    {
        // ignored
    }

    cline++;
} 
        
while (!sr.EndOfStream) {
    var pnum = sr.ReadLine()!;
    try {
        var num = float.Parse(pnum);
        if (num > max) {
            line = cline;
            max = num;
        }
    }
    
    catch (Exception e)
    {
        // ignored
    }

    cline++;
}

Console.WriteLine($"Found max: {max} on line {line}");
sr.Close();