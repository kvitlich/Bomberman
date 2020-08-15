/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2018 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */
using Bomberman.Api;
using Demo.II;

namespace Demo
{
    /// <summary>
    /// This is BombermanAI client demo.
    /// </summary>
    internal class YourSolver : AbstractSolver
    {
        private Bot Markus = null;
        private Mark1 mark1 = null;

        private int kami = 0;
        public YourSolver(string server)
            : base(server)
        {
            Markus = new Bot();
            mark1 = new Mark1();
        }

        /// <summary>
        /// Calls each move to make decision what to do (next move)
        /// </summary>
        protected override string Get(Board board)
        {
            var directions = mark1.GetStep(board);
            //string result = directions[0]+ ", "+ directions[1];
            if (directions.Count >= 2)
            {
                return directions[0].ToString() + directions[1].ToString();
            }
            else if (directions.Count == 1)
            {
                return directions[0].ToString();
            }
            return "0_o";
        }
    }
}
