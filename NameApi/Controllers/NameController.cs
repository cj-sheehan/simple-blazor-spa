using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NameApi.DataAccess.Repositories;
using NameApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {
        private readonly INameRepository _nameRepository;
        private readonly IMapper _mapper;

        public NameController(INameRepository nameRepository, IMapper mapper)
        {
            _nameRepository = nameRepository;
            _mapper = mapper;
        }

        // GET api/name
        [HttpGet(Name = nameof(GetAllAsync))]
        public async Task<ActionResult<IEnumerable<NameResponseModel>>> GetAllAsync()
        {
            var nameModels = await _nameRepository.GetAll();

            if (nameModels is null || nameModels.Count() == 0)
            {
                return NoContent();
            }

            var nameResponseModels = _mapper.Map<IEnumerable<NameResponseModel>>(nameModels);
            return Ok(nameResponseModels);
        }

        // GET api/name/5
        [HttpGet("{id}", Name = nameof(GetByIdAsync))]
        public async Task<ActionResult<NameResponseModel>> GetByIdAsync(int id)
        {
            var nameModel = await _nameRepository.GetNameByIdAsync(id);

            if (nameModel is null)
            {
                return NotFound();
            }

            var nameResponseModel = _mapper.Map<NameResponseModel>(nameModel);
            return Ok(nameResponseModel);
        }

        // POST api/name
        [HttpPost(Name = nameof(PostAsync))]
        public async Task<ActionResult<NameResponseModel>> PostAsync([FromBody] NameCreateRequestModel model)
        {
            var nameModel = await _nameRepository.AddNameAsync(model.Name);
            var responseModel = _mapper.Map<NameResponseModel>(nameModel);

            return CreatedAtRoute(nameof(GetByIdAsync), new { id = responseModel.Id }, responseModel);
        }
    }
}
