/**
 *  Class responsable to store the query filters
 * @returns {number}
 */
export function QueryFilterStore() {
    /**
     * @typedef IDictionary<string, QueryFilter>
     */
    let _queries = {}; 

    return {
        upsert: function (record) {
            _queries[Name] = new QueryFilter(record.Name, record.Operator, record.Value);
        },
        values: function () {
            let result = [];
            for (var key in Object.keys(_queries)) {
                result.push(_queries[key]);
            }
            return result;
        },
        clear: function () {
            _queries = {};
        }
    }

}

export function QueryFilter(name, operator, value){
    this.name = name;
    this.operator = operator;
    this.value = value;
    this.DateLogicalOperator = null;
}