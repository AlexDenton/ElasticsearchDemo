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
    this.state = {
      searchSettings: {
        standardAnalyzer: true,
        snowballAnalayzer: false,
        edgeNGramAnalyzer: false
      }
    }
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
      <div className="search-container">
        <input className="search-input" placeholder="Search" type="text" onChange={this.handleChange} />
        <SearchSettings searchSettings={this.state.searchSettings} />
      </div>
    );
  }
}

class SearchSettings extends Component {

  constructor(props) {
    super(props);
    this.handleInputChange = this.handleInputChange.bind(this);
  }

  handleInputChange(event) {
    this.props.searchSettings[event.target.name] = event.target.checked;
  }

  render() {
    return (
      <div className="search-settings-container">
        <label><input type="checkbox" name="standardAnalyzer" checked={this.props.searchSettings.standardAnalyzer} onChange={this.handleInputChange} /> Standard</label>
        <label><input type="checkbox" name="snowballAnalyzer" checked={this.props.searchSettings.snowballAnalayzer} onChange={this.handleInputChange} /> Snowball</label>
        <label><input type="checkbox" name="edgeNGramAnalyzer" checked={this.props.searchSettings.edgeNGramAnalyzer} onChange={this.handleInputChange} /> Edge NGram</label>
      </div>
    )
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
