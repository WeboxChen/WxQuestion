Ext.define('Wei.model.TreeBase', {
    extend: 'Ext.data.TreeModel',

    //fields: [
    //    'name',
    //    'path',
    //    'size',
    //    {
    //        name: 'leaf',
    //        calculate: function (data) {
    //            return data.root ? false : !data.children;
    //        }
    //    },
    //    {
    //        name: 'text',
    //        calculate: function (data) {
    //            return data.name;
    //        }
    //    }
    //],

    //proxy: {
    //    type: 'ajax',
    //    url: 'data/tree/tree.json'
    //},

    //idProperty: 'path',

    schema: {
        namespace: 'Wei.model'
    }

});