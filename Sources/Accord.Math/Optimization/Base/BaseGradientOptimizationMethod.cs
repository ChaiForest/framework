﻿// Accord Math Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2016
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Math.Optimization
{
    using System;
    using System.Threading;

    /// <summary>
    ///   Base class for gradient-based optimization methods.
    /// </summary>
    /// 
    public abstract class BaseGradientOptimizationMethod : BaseOptimizationMethod
    {
        /// <summary>
        ///   Gets or sets a cancellation token that can be used to
        ///   stop the learning algorithm while it is running.
        /// </summary>
        /// 
        public CancellationToken Token { get; set; }

        /// <summary>
        ///   Gets or sets a function returning the gradient
        ///   vector of the function to be optimized for a
        ///   given value of its free parameters.
        /// </summary>
        /// 
        /// <value>The gradient function.</value>
        /// 
        public Func<double[], double[]> Gradient { get; set; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="BaseGradientOptimizationMethod"/> class.
        /// </summary>
        /// 
        /// <param name="numberOfVariables">The number of free parameters in the optimization problem.</param>
        /// 
        protected BaseGradientOptimizationMethod(int numberOfVariables)
            : base(numberOfVariables)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BaseGradientOptimizationMethod"/> class.
        /// </summary>
        /// 
        /// <param name="numberOfVariables">The number of free parameters in the optimization problem.</param>
        /// <param name="function">The objective function whose optimum values should be found.</param>
        /// <param name="gradient">The gradient of the objective <paramref name="function"/>.</param>
        /// 
        protected BaseGradientOptimizationMethod(int numberOfVariables,
            Func<double[], double> function, Func<double[], double[]> gradient)
            : base(numberOfVariables, function)
        {
            if (gradient == null)
                throw new ArgumentNullException("gradient");

            this.Gradient = gradient;
        }

        /// <summary>
        ///   Finds the maximum value of a function. The solution vector
        ///   will be made available at the <see cref="IOptimizationMethod.Solution"/> property.
        /// </summary>
        /// 
        /// <returns>Returns <c>true</c> if the method converged to a <see cref="IOptimizationMethod.Solution"/>.
        ///   In this case, the found value will also be available at the <see cref="IOptimizationMethod.Value"/>
        ///   property.</returns>
        ///  
        public override bool Maximize()
        {
            if (Gradient == null)
                throw new InvalidOperationException("gradient");

            NonlinearObjectiveFunction.CheckGradient(Gradient, Solution);

            var g = Gradient;

            Gradient = (x) => g(x).Multiply(-1);

            bool success = base.Maximize();

            Gradient = g;

            return success;
        }

        /// <summary>
        ///   Finds the minimum value of a function. The solution vector
        ///   will be made available at the <see cref="IOptimizationMethod.Solution"/> property.
        /// </summary>
        /// 
        /// <returns>Returns <c>true</c> if the method converged to a <see cref="IOptimizationMethod.Solution"/>.
        ///   In this case, the found value will also be available at the <see cref="IOptimizationMethod.Value"/>
        ///   property.</returns>
        ///  
        public override bool Minimize()
        {
            if (Gradient == null)
                throw new InvalidOperationException("gradient");

            NonlinearObjectiveFunction.CheckGradient(Gradient, Solution);

            return base.Minimize();
        }



    }
}
