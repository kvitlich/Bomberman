using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.II
{
    public class MaskBuilder
    {
        private Board board;
        public MaskBuilder(Board board)
        {
            this.board = board;
        }
        public MovementMask GetMovementMask(Point point, int wave)
        {
            return new MovementMask(this.board, wave);
        }

        public DangerMask GetDangerMask(MovementMask movementMask, int wave)
        {
            return new DangerMask(movementMask, this.board, wave);
        }

        public BenefitMask GetBenefitMask(MovementMask movementMask, int wave)
        {
            return new BenefitMask(movementMask, board, wave);
        }
    }
}
