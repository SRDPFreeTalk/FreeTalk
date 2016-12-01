import React, { PropTypes, Component } from 'react'
import Post from '../postlist'

export default class PostList extends Component{
	render(){
		let post = [{
				id: "1",
				nikename: "这是用户名",
				postTile: "",
				postContent: "",
				thumbs: "",
				chat: "",
			}]

		return(
			<div className="main content">
				{post.map(item => 
					<Post 
					  key={item.id}
					  content={...item}
					/>
				)}
			</div>
		)
	}
}