var app = angular.module('date-filter', []);

app.filter('formatDate', function () {
    try {
        return function (input) {
            input = input.replace('/', '').replace('Date', '').replace('/', '').replace('(', '').replace(')', '');
            return new Date(parseInt(input));
        }
    }
    catch (err) {
        return "";
    }
});

app.filter('daterange', ['$filter', function ($filter) {
    return function (conversations, start_date, end_date) {
        var result = [];


        var start_date = (start_date && !isNaN(Date.parse(start_date))) ? new Date(start_date) : 0;
        var end_date = (end_date && !isNaN(Date.parse(end_date))) ? new Date(end_date) : new Date();

        // if the conversations are loaded
        if (conversations && conversations.length > 0 && start_date != 0) {
            $.each(conversations, function (index, conversation) {
                var conversationDate = new Date($filter('date')(new Date(formatDate(conversation.DateSearched)), 'MM/dd/yyyy'));
                if (conversationDate >= start_date && conversationDate <= end_date) {
                    result.push(conversation);
                }
            });

            return result;
        }
        return conversations;
    };
}]);

function formatDate(input) {
    input = input.replace('/', '').replace('Date', '').replace('/', '').replace('(', '').replace(')', '');
    return new Date(parseInt(input));
}