import { GetStaticProps } from "next"

type HomeProps = {

}

const Home = () => {

    return (
        <div>
            <h1>Game</h1>
        </div>
    )
}

export default Home

export const getStaticProps: GetStaticProps<HomeProps, { gameShortId?: string }> = async ({ params }) => {

    console.log("getStaticProps", { shortId: params?.["gameShortId"] })

    // const content = fs.readFileSync(process.cwd() + "/data/recent-games.json").toString()

    // const recentGames = JSON.parse(content) as RecentGames || []

    // console.log(content)
    // console.log(recentGames)

    return {
        props: {
        },
    }
}

export const getStaticPaths = async () => {

    console.log("getStaticPaths")

    return {
        paths: [
            { params: { gameShortId: "mwbl-st-kilda-b-grade-monash-university-2023-07-01-82cdc0a57326" } },
            { params: { gameShortId: "mwbl-st-kilda-b-reserves-monash-university-2023-07-01-fc520d96d5a0" } },
            { params: { gameShortId: "2021-09-11-3" } },
        ],
        fallback: false,
    }
}



