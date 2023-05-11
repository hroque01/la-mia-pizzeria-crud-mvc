using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria_static
{
    public class Ingredients
    {
        public int Id {  get; set; }    
        public string Name { get; set; }
        public List<Pizza> Pizza { get; set; }  
    }
}
