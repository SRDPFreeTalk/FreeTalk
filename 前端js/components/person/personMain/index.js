import React, { PropTypes, Component } from 'react'
import PersonFuc from '../personFuc/'

export default class PersonMain extends Component{
	render(){
		let items = [{
			name: "我的动态",
			isNew: true,
			replyNum: "",	
		}]
		return(
			<div class="person_main">
				{items.map(item=>
					<PersonFuc features={item} />
				)}
			</div>
		)
	}
}