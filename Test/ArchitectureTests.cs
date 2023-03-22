using NetArchTest.Rules;
using NUnit.Framework;

namespace Test
{
    public class ArchitectureTests
    {
        private const string DOMAIN_LAYER_NAME = "Domain";
        private const string APPLICATION_LAYER_NAME = "Application";
        private const string INFRASTRUCTURE_LAYER_NAME = "Infrastructure";
        private const string WEB_LAYER_NAME = "PRO_API";

        [Test]
        public void DomainHasNoDependenciesOnOtherProjects()
        {
            var project = typeof(Domain.Extensions).Assembly;

            var otherProject = new[]
            {
                APPLICATION_LAYER_NAME,
                INFRASTRUCTURE_LAYER_NAME,
                WEB_LAYER_NAME
            };

            var result = Types.InAssembly(project).ShouldNot().HaveDependencyOnAll(otherProject).GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void ApplicationHasNoDependenciesOnOtherProjects()
        {
            var project = typeof(Application.Extensions).Assembly;

            var otherProject = new[]
            {
                INFRASTRUCTURE_LAYER_NAME,
                WEB_LAYER_NAME
            };

            var result = Types.InAssembly(project).ShouldNot().HaveDependencyOnAll(otherProject).GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void InfrastructureHasNoDependenciesOnOtherProjects()
        {
            var project = typeof(Infrastructure.Extensions).Assembly;

            var otherProject = new[]
            {
                WEB_LAYER_NAME
            };

            var result = Types.InAssembly(project).ShouldNot().HaveDependencyOnAll(otherProject).GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }
    }
}