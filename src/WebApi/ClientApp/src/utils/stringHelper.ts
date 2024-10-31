import _ from 'lodash';

export const stringHelper = {
    isEmptyOrNull: function(str: string){
        return (_.isEmpty(str) || _.isNull(str));
    },

    camelize: function(str: string) {
        return _.camelCase(str);
    }
}
