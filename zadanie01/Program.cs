var paramNum = args.Length;
var collectiveString = "";
var repetitons = 0;
for (var i = 0; i < paramNum - 1; i++)
{
    collectiveString += args[i];
}

repetitons = int.Parse(args[paramNum - 1]);

while (repetitons > 0)
{
    Console.Write(collectiveString);
    repetitons--;
}