namespace Sample.Tris.WebApi.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Sample.Tris.Lib.Exceptions;
    using Sample.Tris.Lib.Geometry;
    using Sample.Tris.Lib.Grid;
    using Sample.Tris.Lib.Services;
    using Sample.Tris.WebApi.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class TrianglesController : ControllerBase
    {
        private readonly IMapper _dataMapper;
        private readonly IGridConstraintsFactory _gridContraintsFactory;
        private readonly ITriangleGridQueryService _triangleGridQueryService;

        public TrianglesController(
            IMapper dataMapper,
            IGridConstraintsFactory gridConstaintsFactory,
            ITriangleGridQueryService triangleGridQueryService)
        {
            _dataMapper = dataMapper;
            _gridContraintsFactory = gridConstaintsFactory;
            _triangleGridQueryService = triangleGridQueryService;
        }

        [HttpGet("constraints")]
        public ActionResult<GridConstraints> GetGridSpec()
        {
            return _gridContraintsFactory.GetConstraints();
        }

        [HttpGet("{label}")]
        public ActionResult<TriangleDto> QueryTriangleByLabel(string label)
        {
            try
            {
                var triangle = _triangleGridQueryService.GetTriangleForGridLabel(label);

                return _dataMapper.Map<TriangleDto>(triangle);
            }
            catch (TrisLibValidationException validationEx)
            {
                return UnprocessableEntity(validationEx.Message);
            }
        }

        [HttpGet("query")]
        public ActionResult<TriangleDto> QueryTriangleByPoints(
            [FromQuery, Required] PointDto p1,
            [FromQuery, Required] PointDto p2,
            [FromQuery, Required] PointDto p3)
        {
            try
            {
                var triangle = _triangleGridQueryService.GetTriangleForPoints(
                    _dataMapper.Map<Point>(p1),
                    _dataMapper.Map<Point>(p2),
                    _dataMapper.Map<Point>(p3)
                );

                if (triangle == null)
                {
                    return NotFound("No triangle found");
                }

                return _dataMapper.Map<TriangleDto>(triangle);
            }
            catch (TrisLibValidationException validationEx)
            {
                return UnprocessableEntity(validationEx.Message);
            }
        }
    }
}
