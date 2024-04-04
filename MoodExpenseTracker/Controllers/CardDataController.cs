using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MoodExpenseTracker.Models;

namespace MoodExpenseTracker.Controllers
{
    public class CardDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// A list that returns all the Cards in the system
        /// </summary>
        /// <returns>
        /// All Cards in the system 
        /// </returns>
        ///<example>
        /// GET: api/CardData/ListCards
        /// CURL: curl https://localhost:44307/api/CardData/ListCards
        /// </example>

        // List Cards
        [HttpGet]
        [ResponseType(typeof(Card))]
        [Route("api/CardData/ListCards")]
        public IHttpActionResult ListCards()
        {
            List<Card> Cards = db.Cards.ToList();
            List<CardDto> CardDtos = new List<CardDto>();

            Cards.ForEach(c => CardDtos.Add(new CardDto()
            {
                CardId = c.CardId,
                CardName = c.CardName,
                CardType = c.CardType

            }));

            return Ok(CardDtos);
        }


        /// <summary>
        /// Returns a selected card in the system
        /// </summary>
        /// <param name="id">A selected card id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A card in the system that matches the CardId
        /// </returns>
        /// <example>
        /// GET: api/CardData/FindCard/2
        /// CURL: curl https://localhost:44307/api/CardData/FindCard/2
        /// </example>

        // Find a Card
        [HttpGet]
        [ResponseType(typeof(Card))]
        [Route("api/CardData/FindCard/{id}")]
        public IHttpActionResult FindCard(int id)
        {
            Card Card = db.Cards.Find(id);
            CardDto CardDto = new CardDto()
            {

                CardId = Card.CardId,
                CardName = Card.CardName,
                CardType = Card.CardType

            };
            if (Card == null)
            {
                return NotFound();
            }
            return Ok(CardDto);
        }


        /// <summary>
        /// Updates a particular card in the system with Post data input
        /// </summary>
        /// <param name="id">Represents the card id (primary key)</param>
        /// <param name="card">JSON form data of a card</param>
        /// <returns>
        /// HEADER: 204 (Success, No Conetent Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CardData/UpdateCard/2
        /// FORM Data: Card JSON Object
        /// CURL: curl -d @card.json -H "Content-Type:application/json" https://localhost:44307/api/CardData/UpdateCard/6
        /// </example>

        // Update a card 
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCard(int id, Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != card.CardId)
            {
                return BadRequest();
            }

            db.Entry(card).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add a card to the system with Post data input
        /// </summary>
        /// <param name="card">JSON form data of a card</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Card Id, Card Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CardData/AddCard
        /// FORM Data: Card JSON Object
        /// CURL: curl -d @card.json -H "Content-Type:application/json" https://localhost:44307/api/CardData/AddCard
        /// </example>

        // Add a Card
        [ResponseType(typeof(Card))]
        [HttpPost]
        public IHttpActionResult AddCard(Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cards.Add(card);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = card.CardId }, card);
        }

        /// <summary>
        /// Deletes a card from the system by it's ID
        /// </summary>
        /// <param name="id">Represents the card id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CardData/DeleteCard/2
        /// CURL: curl -d "" https://localhost:44307/api/CardData/DeleteCard/9
        /// </example>

        // Delete a Card
        [ResponseType(typeof(Card))]
        [HttpPost]
        public IHttpActionResult DeleteCard(int id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            db.Cards.Remove(card);
            db.SaveChanges();

            return Ok(card);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardExists(int id)
        {
            return db.Cards.Count(e => e.CardId == id) > 0;
        }
    }
}