using Newtonsoft.Json;

public class ValuesGenerator
{
    private int Seed { get; set; }

    public ValuesGenerator()
    {
        Seed = 100;
    }

    public ValuesGenerator(int? seed)
    {
        Seed = seed ?? 100 ;
    }

    public int Generate()
    {
        return new System.Random(Seed).Next();
    }

    public string Customer(int id)
    {
        return JsonConvert.SerializeObject(new { id = id, Name = "Marcos", Surname = "Vasconcelos" });
    }
}