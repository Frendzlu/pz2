var sum = 0;
var num = Convert.ToInt32(Console.ReadLine());
var amt = 0;
while (num != 0) 
{
    sum += num;
    amt++;
    num = Convert.ToInt32(Console.ReadLine());
}
if (amt == 0)
{
    Console.WriteLine("N/A");
    return;
}
Console.WriteLine(sum/amt);

// Since the files are started from ./bin/Debug/net7.0
var sw = new StreamWriter($"../../../zad02.txt", append:true);
sw.WriteLine(sum/amt);
sw.Close();