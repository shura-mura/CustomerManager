using AutoMapper;
using CustomerManager.BusinessLogic.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public CustomerController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] NewCustomerDto item)
        {
            if (item == null)
                return BadRequest();

            var entity = await _sender.Send(new CreateCustomerCmd(item.FirstName, item.LastName, item.BirthDate));

            if (entity == null)
                return BadRequest();

            return Ok(_mapper.Map<CustomerDto>(entity));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] NewCustomerDto item)
        {
            if (item == null)
                return BadRequest();

            var entity = await _sender.Send(new UpdateCustomerCmd(id, item.FirstName, item.LastName, item.BirthDate));

            if (entity == null)
                return BadRequest();

            return Ok(_mapper.Map<CustomerDto>(entity));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _sender.Send(new DeleteCustomerCmd(id));

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerDto>(entity));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _sender.Send(new GetCustomerCmd(id));
            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerDto>(entity));
        }

        [HttpGet("find")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        public async Task<IEnumerable<CustomerDto>> GetBy(string firstName, string lastName)
        {
            var entities = await _sender.Send(new FindCustomerCmd(firstName, lastName));
            return _mapper.Map<IEnumerable<CustomerDto>>(entities);
        }
    }
}