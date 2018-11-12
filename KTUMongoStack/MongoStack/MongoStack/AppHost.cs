using Funq;
using MongoStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System.Web.Mvc;
using ServiceStack.Mvc;
using MongoStack.Data;
using MongoStack.ServiceInterface.Interfaces;
using Autofac;
using MongoStack.App_Start;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.FluentValidation;
using MongoStack.Core.DTOs;
using MongoDB.Bson;
using MongoStack.ServiceInterface.Services;

namespace MongoStack
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("MongoStack", typeof(MyServices).Assembly) { }

        public override void Configure(Container container)
        {
            SetConfig(new EndpointHostConfig { ServiceStackHandlerFactoryPath = "api" });
            ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
            container.RegisterAs<DataHelper, IHelper>();
            Plugins.Add(new ValidationFeature());
            container.RegisterValidators(typeof(ProductValidator).Assembly);
            container.RegisterValidators(typeof(UpdateProductValidator).Assembly);

            var builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(MongoDbRepository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<SubcategoryService>().As<ISubcategoryService>().SingleInstance();
            builder.RegisterType<ProductService>().As<IProductService>().SingleInstance();
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();

            IContainerAdapter adapter = new AutofacIocAdapter(builder.Build());
            container.Adapter = adapter;
        }

    }
    public class ProductValidator : AbstractValidator<AddProduct>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode("BadRequest"); ;
        }
    }
    
    public class UpdateProductValidator : AbstractValidator<UpdateProduct>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode("BadRequest");
        }
    }
}


