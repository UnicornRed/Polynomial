using System.Collections.Generic;
using System.Text;

namespace Calculating
{
    /// <summary>
    /// Реализует именованные переменные.
    /// </summary>
    public class VarsCalc
    {
        /// <summary>
        /// Имя переменной.
        /// </summary>
        private readonly string id;

        /// <summary>
        /// Значение переменной.
        /// </summary>
        private object varCalc = null;

        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Значение переменной.
        /// </summary>
        public object VarCalc
        {
            get
            {
                return varCalc;
            }
            set
            {
                varCalc = value;
            }
        }

        /// <summary>
        /// Инициализирует переменную по имени, не задавая значения.
        /// </summary>
        /// <param name="id">Имя переменной.</param>
        public VarsCalc(string id) => this.id = id;

        /// <summary>
        /// Инициализирует именнованную переменную по строке с именем и значению переменной.
        /// </summary>
        /// <param name="varCalc">Значение переменной.</param>
        /// <param name="id">Строка, задающая имя полинома.</param>
        public VarsCalc(string id, object varCalc) : this(id) => this.varCalc = varCalc;

        public static VarsCalc GetVarsCalcByKey(string id, List<VarsCalc> varsList)
        {
            foreach (var i in varsList)
                if (i.id == id)
                    return i;

            return null;
        }

        /// <summary>
        /// Проверяет лист именованных переменных на присутствие переменной с переданным именем.
        /// </summary>
        /// <param name="id">Имя переменной.</param>
        /// <param name="varsList">Лист именованных переменных.</param>
        /// <returns>true, если переменной в списке нет, false в остальных случаях.</returns>
        public static bool IsIdFree(string id, List<VarsCalc> varsList)
        {
            foreach (var i in varsList)
                if (i.id == id)
                    return false;

            return true;
        }

        /// <summary>
        /// Переопределение преобразования именованной переменной в строку.
        /// </summary>
        /// <returns>Строку, соответствующую данной именованной переменной.</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder("");

            str.Append(id + " = " + base.ToString());

            return str.ToString();
        }
    }
}
