using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        public ICustomLogger Logger;

        public PizzaController(ICustomLogger logger)
        {
            Logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizze = db.Pizza.OrderBy(pizza => pizza.Id).ToList<Pizza>();

                if (pizze.Count == 0)
                    return View("Error", "Non ci sono pizze!");

                return View(pizze);
            }

        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza == null)
                    return View("Error", "Nessuna pizza trovata con questo ID!");

                return View(pizza);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList();
                    data.Categories = categories;
                    return View(data);
                }
            }

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = new Pizza();
                pizza.Nome = data.Pizza.Nome;
                pizza.Descrizione = data.Pizza.Descrizione;
                pizza.Prezzo = data.Pizza.Prezzo;
                pizza.Img = data.Pizza.Img;

                pizza.CategoryId = data.Pizza.CategoryId;

                db.Pizza.Add(pizza);
                db.SaveChanges();

                Logger.WriteLog($"Elemento '{pizza.Nome}' creato!");


                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext context = new PizzaContext())
            {
                List<Category> categories = context.Categories.ToList();

                PizzaFormModel model = new PizzaFormModel();
                model.Pizza = new Pizza();
                model.Categories = categories;

                return View("Create", model);
            }
            
        }
         
        [HttpGet]
        public IActionResult Update(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza == null)
                    return NotFound();

                else
                {
                    List<Category> categories = db.Categories.ToList();
                    PizzaFormModel model = new PizzaFormModel();
                    model.Pizza = pizza; 
                    model.Categories = categories;
                    return View(model);
                }
                    
            }
             
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(long id, Pizza data)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", data);
            }

            using(PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza != null)
                {
                    pizza.Nome = data.Nome;
                    pizza.Descrizione = data.Descrizione;
                    pizza.Prezzo = data.Prezzo;


                    db.SaveChanges();

                    Logger.WriteLog($"Elemento '{pizza.Nome}' modificato!");

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }


            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza != null)
                {
                    db.Pizza.Remove(pizza);

                    db.SaveChanges();

                    Logger.WriteLog($"Elemento '{pizza.Nome}' eliminato!");

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}

   
