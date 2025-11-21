namespace Script.Core.Entity
{
    public class Hero : BaseEntity
    {
        public override float F_GetMoveSpeed()
        {
            return 2;
        }

        public override enEntityType F_GetEntityType()
        {
            return enEntityType.Hero;
        }
    }
}