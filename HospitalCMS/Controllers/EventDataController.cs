using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalCMS.Models;

namespace HospitalCMS.Controllers
{
    public class EventDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [ResponseType(typeof(EventDto))]

        /// <summary>
        /// Returns all Event in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Event in the database
        /// </returns>
        /// <example>
        // GET: api/EventData/ListEvents
        /// </example>
        
        public IHttpActionResult ListEvents()
        {
            List<Event> Event = db.Events.ToList();
            List<EventDto> EventDto = new List<EventDto>();

            Event.ForEach(a => EventDto.Add(new EventDto()
            {
                EventId = a.EventId,
                Title = a.Title,
                Date = a.Date,
                HasPic = a.HasPic,
                PicExtension = a.PicExtension,
                Description = a.Description,
                Department = new DepartmentDto() { DepartmentId = a.DepartmentId, Name = a.Department.Name }
            }));
            return Ok(EventDto);
        }

        [HttpGet]
        [ResponseType(typeof(Event))]

        /// <summary>
        /// Returns details of the Event by Event id
        /// </summary>
        /// <param name="id">Event primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Event in the system matching up to the Event ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // GET: api/EventData/FindEvent/1
        /// </example>
        public IHttpActionResult FindEvent(int id)

        {
            Event Event = db.Events.Include(x => x.Volunteers).Where(x => x.EventId == id).FirstOrDefault();
            EventDto EventDto = new EventDto()
            {
                EventId = Event.EventId,
                Title = Event.Title,
                Date = Event.Date,
                HasPic = Event.HasPic,
                PicExtension = Event.PicExtension,
                Description= Event.Description,
                Articles = Event.Articles,
                DepartmentId = Event.DepartmentId,
                Department = new DepartmentDto() { DepartmentId = Event.DepartmentId, Name = Event.Department.Name }
            };
            EventDto.Volunteers = new List<VolunteerDto>();
            if (Event.Volunteers!=null)
            {
                foreach (Volunteer volunteer in Event.Volunteers)
                {
                    EventDto.Volunteers.Add(new VolunteerDto()
                    {
                        VolunteerName = volunteer.VolunteerName,
                        VolunteerId = volunteer.VolunteerId
                    });
                }
            }
            if (Event == null)
            {
                return NotFound();
            }

            return Ok(EventDto);
        }

         /// <summary>
        /// Create an Event to the system
        /// </summary>
        /// <param name="Event">JSON form data of Event</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:Event ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/EventData/AddEvent
        /// </example>
        [HttpPost]
        
        [ResponseType(typeof(Event))]
        public IHttpActionResult AddEvent(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = @event.EventId }, @event);
        }

        /// <summary>
        /// Edit a particular Event in the system with POST Data input
        /// </summary>
        /// <param name="id">Event primary key</param>
        /// <param name="Event">JSON form data of an Event</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // POST: api/EventData/EditEvent/5
        /// FORM DATA: Event JSON Object
        /// </example>
        
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult EditEvent(int id, Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.EventId)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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


        [HttpPost]
        public IHttpActionResult UploadEventPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {

                int numfiles = HttpContext.Current.Request.Files.Count;

                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var eventPic = HttpContext.Current.Request.Files[0];
                    if (eventPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(eventPic.FileName).Substring(1);
                        if (valtypes.Contains(extension, StringComparer.InvariantCultureIgnoreCase))
                        {
                            try
                            {
                                string fn = id + "." + extension;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Events/"), fn);

                                eventPic.SaveAs(path);

                                haspic = true;
                                picextension = extension;

                                Event selectedEvent = db.Events.Find(id);
                                selectedEvent.HasPic = haspic;
                                selectedEvent.PicExtension = extension;
                                db.Entry(selectedEvent).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                return BadRequest();
                            }
                        }
                    }

                }
                else
                {
                    Doctor SelectedDoctor = db.Doctors.Find(id);
                    SelectedDoctor.DoctorHasPic = haspic;
                    SelectedDoctor.PicExtension = null;
                    db.Entry(SelectedDoctor).State = EntityState.Modified;

                    db.SaveChanges();
                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

        }



        /// <summary>
        /// Deletes an Event from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of Event</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/EventData/5
        /// FORM DATA: (empty)
        /// </example>
        [HttpPost]
        
        [ResponseType(typeof(Event))]
        [Authorize]
        public IHttpActionResult DeleteEvent(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            db.SaveChanges();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventId == id) > 0;

        }
    }
}
    /*public class EventDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EventData
        public IQueryable<Event> GetEvents()
        {
            return db.Events;
        }

        // GET: api/EventData/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult GetEvent(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/EventData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEvent(int id, Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.EventId)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/EventData
        [ResponseType(typeof(Event))]
        public IHttpActionResult PostEvent(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = @event.EventId }, @event);
        }

        // DELETE: api/EventData/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult DeleteEvent(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            db.SaveChanges();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventId == id) > 0;
        }
    }
}*/