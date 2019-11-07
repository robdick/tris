namespace Sample.Tris.WebApi.Controllers.Tests
{
    using System;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Sample.Tris.Lib.Exceptions;
    using Sample.Tris.Lib.Geometry;
    using Sample.Tris.Lib.Grid;
    using Sample.Tris.Lib.Services;
    using Sample.Tris.WebApi.Configuration;
    using Sample.Tris.WebApi.Controllers;
    using Sample.Tris.WebApi.Models;
    using Xunit;

    public class TrianglesControllerTests
    {
        private readonly GridAddress CANONICAL_TEST_GRID_ADDRESS;
        private readonly Triangle CANONICAL_TEST_TRIANGLE;
        private readonly GridConstraints CANONICAL_TEST_GRID_CONSTRAINTS;

        private readonly IMapper _mapper;
        private readonly Mock<ITriangleGridQueryService> _triangleGridQueryService;
        private readonly Mock<IGridConstraintsFactory> _gridConstraintsFactoryMock;
        private readonly TrianglesController _trianglesController;

        public TrianglesControllerTests()
        {
            CANONICAL_TEST_GRID_CONSTRAINTS = new GridConstraints(6, 6, 10);
            CANONICAL_TEST_GRID_ADDRESS = new GridAddress(1, 1, "LABEL");
            CANONICAL_TEST_TRIANGLE = new Triangle(
                new Point(0, 0),
                new Point(10, 10),
                new Point(0, 10),
                CANONICAL_TEST_GRID_ADDRESS
            );

            _mapper = AutomapperConfiguration.CreateMapper();

            _gridConstraintsFactoryMock = new Mock<IGridConstraintsFactory>();
            _gridConstraintsFactoryMock
                .Setup(x => x.GetConstraints())
                .Returns(CANONICAL_TEST_GRID_CONSTRAINTS);

            _triangleGridQueryService = new Mock<ITriangleGridQueryService>();
            _triangleGridQueryService
                .Setup(x => x.GetTriangleForGridLabel(It.IsAny<string>()))
                .Returns(CANONICAL_TEST_TRIANGLE);



            _trianglesController = new TrianglesController(
                _mapper,
                _gridConstraintsFactoryMock.Object,
                _triangleGridQueryService.Object
            );
        }

        #region GetGridSpec

        [Fact]
        public void GetGridSpec_Returns_GridConstraints()
        {
            var result = _trianglesController.GetGridSpec();

            Assert.Equal(CANONICAL_TEST_GRID_CONSTRAINTS, result.Value);
        }

        #endregion

        #region QueryTriangleByLabel

        [Fact]
        public void QueryTriangleByLabel_CallsGridQueryService()
        {
            _trianglesController.QueryTriangleByLabel(CANONICAL_TEST_GRID_ADDRESS.Label);

            _triangleGridQueryService.Verify(
                x => x.GetTriangleForGridLabel(CANONICAL_TEST_GRID_ADDRESS.Label), Times.Once
            );
        }

        [Fact]
        public void QueryTriangleByLabel_MapsTriangleFromQueryService()
        {
            var result = _trianglesController.QueryTriangleByLabel(CANONICAL_TEST_GRID_ADDRESS.Label);
            var triangleDto = result.Value;

            AssertTriangle(CANONICAL_TEST_TRIANGLE, triangleDto);
        }

        [Fact]
        public void QueryTriangleByLabel_NotFound_Result_With_ValidationException()
        {
            _triangleGridQueryService
                .Setup(x => x.GetTriangleForGridLabel(CANONICAL_TEST_GRID_ADDRESS.Label))
                .Throws(new TrisLibValidationException("Error Message"));

            var result = _trianglesController.QueryTriangleByLabel("LABEL");

            Assert.IsType<UnprocessableEntityObjectResult>(result.Result);
            Assert.Null(result.Value);
        }

        #endregion

        #region QueryTriangleByPoints

        [Fact]
        public void QueryTriangleByPoints_CallsGridQueryService()
        {
            _trianglesController.QueryTriangleByPoints(
                It.IsAny<PointDto>(), It.IsAny<PointDto>(), It.IsAny<PointDto>()
            );

            _triangleGridQueryService.Verify(
                x => x.GetTriangleForPoints(It.IsAny<Point>(), It.IsAny<Point>(), It.IsAny<Point>()),
                Times.Once
            );
        }

        [Fact]
        public void QueryTriangleByPoints_MapsTriangleFromQueryService()
        {
            _triangleGridQueryService
                .Setup(x => x.GetTriangleForPoints(It.IsAny<Point>(), It.IsAny<Point>(), It.IsAny<Point>()))
                .Returns(CANONICAL_TEST_TRIANGLE);

            var result = _trianglesController.QueryTriangleByPoints(It.IsAny<PointDto>(), It.IsAny<PointDto>(), It.IsAny<PointDto>());
            var triangleDto = result.Value;

            AssertTriangle(CANONICAL_TEST_TRIANGLE, triangleDto);
        }

        [Fact]
        public void QueryTriangleByPoints_UnprocessableEntityResult_WhenExceptionRaised()
        {
            _triangleGridQueryService
                .Setup(x => x.GetTriangleForPoints(It.IsAny<Point>(), It.IsAny<Point>(), It.IsAny<Point>()))
                .Throws(new TrisLibValidationException("Error Message"));

            var result = _trianglesController.QueryTriangleByPoints(
                It.IsAny<PointDto>(), It.IsAny<PointDto>(), It.IsAny<PointDto>()
            );

            Assert.IsType<UnprocessableEntityObjectResult>(result.Result);
            Assert.Null(result.Value);
        }

        #endregion

        #region Private helpers

        private void AssertTriangle(Triangle expectedTriangle, TriangleDto actualTriangleDto)
        {
            AssertPoint(expectedTriangle.P1, actualTriangleDto.P1);
            AssertPoint(expectedTriangle.P2, actualTriangleDto.P2);
            AssertPoint(expectedTriangle.P3, actualTriangleDto.P3);

            Assert.Equal(expectedTriangle.GridAddress.Row, actualTriangleDto.GridRow);
            Assert.Equal(expectedTriangle.GridAddress.Column, actualTriangleDto.GridColumn);
            Assert.Equal(expectedTriangle.GridAddress.Label, actualTriangleDto.GridLabel);
        }

        private void AssertPoint(Point expectedPoint, PointDto actualPointDto)
        {
            Assert.Equal(expectedPoint.X, actualPointDto.X);
            Assert.Equal(expectedPoint.Y, actualPointDto.Y);
        }

        #endregion
    }
}
