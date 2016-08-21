// 
// Outdoor Tracker - Copyright(C) 2016 Meinard Jean-Richard
//  
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace OutdoorTracker.Common
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