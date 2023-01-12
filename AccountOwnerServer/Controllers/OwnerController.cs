using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
//using Repository;
//using System.Linq.Expressions;

using DynamicLinq;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private ILoggerManager _logger; 
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        
        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper) 
        { 
            _logger = logger; 
            _repository = repository; 
            _mapper = mapper;
        }

        [HttpGet] 
        public IActionResult GetAllOwners(bool includeRelations = true,
                                          bool UseLazyLoading = true,
                                          bool UseMapster = true) 
        { 
            try 
            {
                var owners = _repository.Owner.GetAllOwners(false);

                if ((false == includeRelations) || (false == UseLazyLoading))
                {
                    _repository.Owner.DisableLazyLoading();
                }
                else  // true == includeRelations && true == UseLazyLoading 
                {
                    _repository.Owner.EnableLazyLoading();
                }

                if (true == UseLazyLoading)
                {
                    owners = _repository.Owner.FindAll();
                }
                else
                {
                    owners = _repository.Owner.GetAllOwners(includeRelations); 
                }

                _logger.LogInfo($"Returned all owners from database.");

                // For at sikre at alle andre Endpoints bruger Lazy Loading !!!
                _repository.Owner.EnableLazyLoading();

                if (true == UseMapster)
                {
                    var ownersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
                    return Ok(ownersResult);
                }
                else
                {
                    return Ok(owners);
                }
            } 
            catch (Exception ex) 
            { 
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}"); 
                
                return StatusCode(500, "Internal server error"); 
            } 
        }

        [HttpPost("GetOwnersByConditions")]
        public IActionResult GetOwnersByConditions([FromBody] List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List)
        {
            try
            {
                //var owners = _repository.Owner.GetOwnersByConditions(WebApiDynamicCommunication_Object_List);
                var owners = _repository.GetOwnersByConditions<Owner>(WebApiDynamicCommunication_Object_List);
                _logger.LogInfo($"Returned all owners from database.");

                return Ok(owners);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("GetTestDynamicsByConditions")]
        public IActionResult GetTestDynamicsByConditions([FromBody] List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List)
        {
            try
            {
                var testDynamics = _repository.GetOwnersByConditions<TestDynamic>(WebApiDynamicCommunication_Object_List);
                _logger.LogInfo($"Returned all owners from database.");

                return Ok(testDynamics);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);
                if (owner is null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with id: {id}");

                    var ownerResult = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/account")]
        public IActionResult GetOwnerWithDetails(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerWithDetails(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with details for id: {id}");

                    var ownerResult = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateOwner([FromBody] OwnerForCreationDto owner)
        {
            try
            {
                if (owner is null)
                {
                    _logger.LogError("Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = _mapper.Map<Owner>(owner);

                _repository.Owner.Create(ownerEntity);
                //_repository.Owner.CreateOwner(ownerEntity);
                _repository.Save();

                var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

                return CreatedAtRoute("OwnerById", new { id = createdOwner.Id }, createdOwner);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOwner(Guid id, [FromBody] OwnerForUpdateDto owner)
        {
            try
            {
                if (owner is null)
                {
                    _logger.LogError("Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = _repository.Owner.GetOwnerById(id);
                if (ownerEntity is null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(owner, ownerEntity);

                _repository.Owner.UpdateOwner(ownerEntity);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOwner(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                if (_repository.Account.AccountsByOwner(id).Any())
                {
                    _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                }

                _repository.Owner.DeleteOwner(owner);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
