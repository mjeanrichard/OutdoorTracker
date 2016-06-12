using System;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace OutdoorTraker.Common
{
	public class TypedParameterOverride<TParam> : ResolverOverride
	{
		private readonly InjectionParameterValue _value;

		public TypedParameterOverride(TParam value)
		{
			_value = InjectionParameterValue.ToParameter(value);
		}

		public override IDependencyResolverPolicy GetResolver(IBuilderContext context, Type dependencyType)
		{
			if (dependencyType == typeof(TParam))
			{
				return _value.GetResolverPolicy(dependencyType);
			}
			return null;
		}
	}
}