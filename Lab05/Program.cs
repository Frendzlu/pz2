using lab05;

var cpp = new ConsumerProducerProblem(5, 3);
var dfw = new DirectoryFileWatcher(@"C:\Users\Frendzlu\Desktop\");
var pf = new PastaFinder(@"C:\Users\Frendzlu\Desktop", ".pdf");
var mtss = new MultipleThreadStartStopper(10);