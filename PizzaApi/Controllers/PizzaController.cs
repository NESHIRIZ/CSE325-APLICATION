using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Models;
using PizzaApi.Services;

namespace PizzaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController : ControllerBase
{
    private readonly PizzaService _pizzaService;

    public PizzaController(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<Pizza>> Get()
    {
        var pizzas = _pizzaService.GetAll();
        return Ok(pizzas);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = _pizzaService.Get(id);
        return pizza is null ? NotFound() : Ok(pizza);
    }

    [HttpPost]
    public ActionResult<Pizza> Post([FromBody] Pizza pizza)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var createdPizza = _pizzaService.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = createdPizza.Id }, createdPizza);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] Pizza pizza)
    {
        if (pizza.Id != id)
        {
            return BadRequest();
        }

        var updated = _pizzaService.Update(pizza);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _pizzaService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
