function AppViewModel() {
    var self = this;

    self.keywords = ko.observable('');
    self.url = ko.observable('');
    self.selectedSearchEngine = ko.observable('');
    self.searchEngines = ko.observableArray(['Google']);
    self.searchResults = ko.observableArray([]);

    self.search = function() {
        var data = {
            keywords: self.keywords(),
            url: self.url(),
            searchEngine: self.selectedSearchEngine()
        };

        fetch('https://localhost:5001/Search', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
        .then(result => {
            self.searchResults.push(result);
        })
        .catch(error => {
            console.error('Error fetching search results:', error);
        });
    };
}

ko.applyBindings(new AppViewModel());
