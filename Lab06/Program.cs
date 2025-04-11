// See https://aka.ms/new-console-template for more information

using lab06;

var c1 = new Client(5001, 24);
var c2 = new Client(5002, -1);

var s1 = new Server(5001, 24);
var s2 = new Server(5002, -1);
//
// s1.StartPing();
// c1.StartPing();
//
// s2.StartPing();
// c2.StartPing();

s2.Start();
c2.Start();