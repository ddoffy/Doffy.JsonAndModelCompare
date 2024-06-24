namespace Doffy.JsonAndModelCompare;
class Model
{
    public string Name { get; set; }
    public int Age { get; set; }
    public SubModel SubModel { get; set; }
    public SubArrayModel SubArrayModel { get; set; }
}

class SubModel
{
    public string Name { get; set; }
    public int Age { get; set; }
    public SubSubModel SubSubModel { get; set; }
}

class SubSubModel
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class SubArrayModel
{
    public string Name { get; set; }
    public int Age { get; set; }
    public SubModel[] SubModels { get; set; }
}
