namespace Sample.Tris.WebApi.Configuration
{
    using Microsoft.Extensions.DependencyInjection;
    using AutoMapper;
    using Sample.Tris.WebApi.Models;
    using Sample.Tris.Lib.Geometry;

    /// <summary>
    ///
    /// </summary>
    public static class AutomapperConfiguration
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PointDto, Point>()
                    .ReverseMap();
                cfg.CreateMap<Triangle, TriangleDto>()
                    .ForMember(m => m.GridLabel, m => m.MapFrom(r => r.GridAddress.Label))
                    .ForMember(m => m.GridRow, m => m.MapFrom(r => r.GridAddress.Row))
                    .ForMember(m => m.GridColumn, m => m.MapFrom(r => r.GridAddress.Column))
                    .ForMember(m => m.P1, m => m.MapFrom(r => r.P1))
                    .ForMember(m => m.P2, m => m.MapFrom(r => r.P2))
                    .ForMember(m => m.P3, m => m.MapFrom(r => r.P3));
            });

            return configuration.CreateMapper();
        }
    }
}
