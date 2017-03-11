import React, { Component } from 'react';
import './App.css';

class App extends Component {

  constructor(props) {
    super(props);
    this.state = {
      searchResults: []
    }
    this.handleSearchResultsChange = this.handleSearchResultsChange.bind(this);
  }

  handleSearchResultsChange(searchResults) {
    this.setState({
      "searchResults": searchResults
    });
  }

  render() {
    const searchResults = this.state.searchResults;

    return (
      <div className="wrapper">
        <div className="left">
          <SearchInput onSearchResultsChange={this.handleSearchResultsChange}/>
        </div>
        <div className="right">
          <div className="right-inner">
            <SearchResults searchResults={searchResults}/>
          </div>
        </div>
		  </div>
    );
  }
}

class SearchInput extends Component {
  constructor(props) {
    super(props);
    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(event) {
    var requestHeaders = new Headers();
    requestHeaders.append("Content-Type", "application/json");
    
    var requestBody = 
      JSON.stringify({
        "query": event.target.value
      });

    var requestInit = { 
      method: 'POST',
      headers: requestHeaders,
      mode: 'cors',
      cache: 'default',
      body: requestBody
    };

    var request = new Request("http://localhost:12201/search", requestInit);

    fetch(request)
      .then(function(response) {
        return response.json();
      })
      .then(function(jsonResponse) {
        console.log(jsonResponse);
        this.props.onSearchResultsChange(jsonResponse.results);
      }.bind(this));
  }

  render () {
    return (
      <input className="search-input" placeholder="Search" type="text" onChange={this.handleChange} />
    );
  }
}

class SearchResults extends Component {
  
  render () {
    const searchResults = this.props.searchResults;

    const searchResultItems = searchResults.map((searchResult) => 
      <div key={searchResult.id} className="search-result">
        {searchResult.name}
			</div>
    )
    return (
      <div>{searchResultItems}</div>
    );
  }
}

export default App;
